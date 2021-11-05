using Newtonsoft.Json;
using System;

namespace BlazorJSONEditor
{
    public class StringConverter : JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Replace("\\n", "\n"));
        }
    }
}
