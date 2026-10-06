using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Apps.AdobeWorkfront.Utils;

public static class CustomFieldValueParser
{
    public static List<string> ToValues(object? raw) => raw switch
    {
        null => [],
        string s => [RichTextToPlainTextConverter.IsRichText(s) ? RichTextToPlainTextConverter.ConvertToPlainText(s) : s],
        JArray array => array.SelectMany(x => ToValues((x as JValue)?.Value ?? x.ToString())).ToList(),
        IFormattable f => [f.ToString(null, CultureInfo.InvariantCulture)],
        _ => [raw.ToString() ?? string.Empty]
    };
}