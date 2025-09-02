using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class ProjectWithTasksResponse : ProjectResponse
{
    [Display("Tasks"), JsonProperty("tasks")]
    public List<TaskSmallResponse> Tasks { get; set; } = new();
}