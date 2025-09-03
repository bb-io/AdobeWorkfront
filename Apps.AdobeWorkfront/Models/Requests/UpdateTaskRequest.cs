using Apps.AdobeWorkfront.Handlers;
using Apps.AdobeWorkfront.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class UpdateTaskRequest : TaskRequest
{
    [Display("Task name")]
    public string? Name { get; set; }
    
    [Display("Status"), StaticDataSource(typeof(TaskStatusDataHandler))]
    public string? Status { get; set; }
    
    [Display("Priority")]
    public int? Priority { get; set; }
    
    [Display("Assignee IDs"), DataSource(typeof(UserDataHandler))]
    public IEnumerable<string>? AssigneeIds { get; set; }
}
