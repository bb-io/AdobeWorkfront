using Apps.AdobeWorkfront.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.AdobeWorkfront.Models.Requests;

public class TaskStatusOptionalRequest
{
    [Display("Task status"), StaticDataSource(typeof(TaskStatusDataHandler))]
    public string? TaskStatus { get; set; }
}