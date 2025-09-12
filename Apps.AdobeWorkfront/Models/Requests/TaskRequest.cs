using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class TaskRequest
{
    [Display("Task ID"), DataSource(typeof(TaskDataHandler))]
    public string TaskId { get; set; } = string.Empty;
}