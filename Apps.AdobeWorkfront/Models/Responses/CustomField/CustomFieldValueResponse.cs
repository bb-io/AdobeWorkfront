using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses.CustomField;

public class CustomFieldValueResponse(string name, object? raw)
{
    [Display("Custom field name")]
    public string Name { get; set; } = name;

    [Display("Custom field value")]
    public string CustomFieldValue { get; set; } = CustomFieldValueParser.ToText(raw);

    [Display("Custom field values")]
    public List<string> CustomFieldValues { get; set; } = CustomFieldValueParser.ToValues(raw);
}