using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses;

public class StringResponse(string customFieldValue)
{
    [Display("Custom field value")]
    public string CustomFieldValue { get; set; } = customFieldValue;
}