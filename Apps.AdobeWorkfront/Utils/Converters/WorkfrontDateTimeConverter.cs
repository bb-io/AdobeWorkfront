using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Utils.Converters;

public class WorkfrontDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? ReadJson(
        JsonReader reader, 
        Type objectType, 
        DateTime? existingValue, 
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        return WorkfrontDateTime.TryParse(reader.Value?.ToString(), out var parsed) ? parsed : default;
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        try
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
        }
        catch
        {
            writer.WriteNull();
        }
    }
}