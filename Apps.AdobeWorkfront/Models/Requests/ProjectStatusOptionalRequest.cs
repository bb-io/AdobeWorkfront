using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Requests;

public class ProjectStatusOptionalRequest
{
    [Display("Project status")]
    public string? ProjectStatus { get; set; }
}