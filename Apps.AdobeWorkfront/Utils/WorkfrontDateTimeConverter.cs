using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Utils;

public class WorkfrontDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var raw = reader.Value?.ToString();
        if (string.IsNullOrEmpty(raw))
        {
            return default;
        }

        raw = raw.Replace(":000", ".000");
        return DateTime.Parse(raw, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
    }
}
