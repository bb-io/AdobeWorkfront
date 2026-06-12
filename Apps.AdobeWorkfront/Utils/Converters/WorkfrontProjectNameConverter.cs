using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.AdobeWorkfront.Utils.Converters;

public class WorkfrontProjectNameConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }

    public override object ReadJson(
        JsonReader reader, 
        Type objectType, 
        object? existingValue, 
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return string.Empty;

        JObject obj = JObject.Load(reader);
        return obj["name"]?.ToString() ?? string.Empty;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}