using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses;

public class SearchProjectsResponse(List<ProjectResponse> projects)
{
    public List<ProjectResponse> Projects { get; set; } = projects;
    
    [Display("Total count")]
    public int TotalCount { get; set;  } = projects.Count;
}