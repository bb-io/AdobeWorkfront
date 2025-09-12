using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class ProjectOptionalRequest
{
    [Display("Project ID"), DataSource(typeof(ProjectDataHandler))]
    public string? ProjectId { get; set; }
}