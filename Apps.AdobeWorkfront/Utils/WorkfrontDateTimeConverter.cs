using Newtonsoft.Json;
using System.Globalization;

namespace Apps.AdobeWorkfront.Utils;

public class WorkfrontDateTimeConverter : JsonConverter<DateTime?>
{
    private static readonly string[] Formats =
    [
        "yyyy-MM-ddTHH:mm:ss:fffzzz",
        "yyyy-MM-ddTHH:mm:ss.fffzzz",
        "dd.MM.yyyy HH:mm:ss"
    ];
    
    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var raw = reader.Value?.ToString();
        if (string.IsNullOrEmpty(raw))
        {
            return default;
        }

        if (DateTime.TryParseExact(raw, Formats, CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind, out var parsed))
        {
            return parsed;
        }

        throw new FormatException($"Invalid Workfront datetime format: {raw}");
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        
        writer.WriteValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
    }
}