using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class CreateTaskRequest
{
    [Display("Project ID"), DataSource(typeof(ProjectDataHandler))]
    public string ProjectId { get; set; } = string.Empty;
    
    [Display("Task name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Assignee IDs"), DataSource(typeof(UserDataHandler))]
    public IEnumerable<string>? AssigneeIds { get; set; }
    
    [Display("Priority")]
    public int? Priority { get; set; }
}