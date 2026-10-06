using System.Globalization;

namespace Apps.AdobeWorkfront.Utils;

public static class WorkfrontDateTime
{
    private static readonly string[] Formats =
    [
        "yyyy-MM-ddTHH:mm:ss:fffzzz",
        "yyyy-MM-ddTHH:mm:ss.fffzzz",
        "dd.MM.yyyy HH:mm:ss",
        "yyyy-MM-dd"
    ];

    public static bool TryParse(string? raw, out DateTime parsed) =>
        DateTime.TryParseExact(raw, Formats, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsed);
}