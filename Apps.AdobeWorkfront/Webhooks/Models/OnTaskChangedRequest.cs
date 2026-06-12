using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Webhooks.Models;

public class OnTaskChangedRequest
{
    [Display("Task name contains")]
    public string? TaskNameContains { get; set; }
}