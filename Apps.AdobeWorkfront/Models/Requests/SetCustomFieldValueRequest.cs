using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Requests;

public class SetCustomFieldValueRequest : CustomFieldRequest
{
    [Display("Custom field value")]
    public string CustomFieldValue { get; set; } = string.Empty;
}