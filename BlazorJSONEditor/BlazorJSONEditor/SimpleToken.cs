using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BlazorJSONEditor
{
    public class SimpleToken
    {
        public JsonToken Type;
        public string Path;
        public string Text;
        public string Style;

        public static IEnumerable<SimpleToken> CreateStringTokens(string value, int lineNumber, int linePosition, string path, string json)
        {
            var lines = value.Split("\n").Select(s => s.Replace("\"", "\\\"")).ToArray();

            if (lines.Length == 1)
            {
                var style = "position: absolute; left: " + (linePosition - lines[0].Length - 1) + "ch; top: " + (lineNumber - 1) * 14 + "px;";
                yield return new SimpleToken { Text = lines[0], Style = style, Type = JsonToken.String, Path = path };
            }
            else
            {
                var jsonLines = json.Split("\n").ToArray();
                for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                {
                    var line = lines[lineIndex];
                    string style;
                    if (lineIndex == 0)
                    {
                        var offset = jsonLines[lineNumber - lines.Length + lineIndex].Length - line.Length;
                        style = "position: absolute; left: " + offset + "ch; top: " + (lineNumber - lines.Length + lineIndex) * 14 + "px;";
                    }
                    else
                    {
                        style = "position: absolute; left: 0ch; top: " + (lineNumber - lines.Length + lineIndex) * 14 + "px;";
                    }
                    yield return new SimpleToken { Text = line, Style = style, Type = JsonToken.String, Path = path };
                }
            }
        }

        public SimpleToken()
        {
        }

        public SimpleToken(object value, JsonToken type, int lineNumber, int linePosition, string path)
        {
            Type = type;
            Path = path;

            var startLine = lineNumber - 1;
            var startPosition = linePosition - 1;

            switch (Type)
            {
                case JsonToken.StartObject:
                    Text = "{";
                    break;

                case JsonToken.StartArray:
                    Text = "[";
                    break;

                case JsonToken.EndObject:
                    Text = "}";
                    break;

                case JsonToken.EndArray:
                    Text = "]";
                    break;

                case JsonToken.PropertyName:
                    Text = (string)value;
                    startPosition = linePosition - ((string)value).Length - 2;
                    break;

                case JsonToken.Comment:
                    Text = (string)value;
                    startPosition = linePosition - ((string)value).Length;
                    break;

                case JsonToken.Boolean:
                    Text = (bool)value ? "true" : "false";
                    startPosition = linePosition - ((bool)value ? 4 : 5);
                    break;

                case JsonToken.Null:
                    Text = "null";
                    startPosition = linePosition - 4;
                    break;

                case JsonToken.Raw:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Date:
                    Text = value.ToString();
                    startPosition = linePosition - value.ToString().Length;
                    break;

                case JsonToken.None:
                case JsonToken.StartConstructor:
                case JsonToken.Undefined:
                case JsonToken.EndConstructor:
                case JsonToken.Bytes:
                default:
                    Text = value.ToString();
                    break;
            }

            Style = "position: absolute; left: " + startPosition + "ch; top: " + startLine * 14 + "px;";
        }
    }
}
