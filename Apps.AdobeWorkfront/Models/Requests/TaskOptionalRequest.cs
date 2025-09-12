using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class TaskOptionalRequest
{
    [Display("Task ID"), DataSource(typeof(TaskDataHandler))]
    public string? TaskId { get; set; } 
}