using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses;

public class SearchProjectsResponse(List<ProjectWithTasksResponse> projects)
{
    public List<ProjectWithTasksResponse> Projects { get; set; } = projects;
    
    [Display("Total count")]
    public int TotalCount { get; set;  } = projects.Count;
}