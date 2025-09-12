using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class CustomFieldRequest : ParentRequest
{
    [Display("Custom field name"), DataSource(typeof(CustomFieldDataHandler))]
    public string CustomField { get; set; } = string.Empty;
}