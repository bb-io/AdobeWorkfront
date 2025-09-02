using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses;

public class SearchTasksResponse(List<TaskResponse> tasks)
{
    public List<TaskResponse> Tasks { get; set; } = tasks;
    
    [Display("Total count")]
    public int TotalCount { get; set;  } = tasks.Count;
}