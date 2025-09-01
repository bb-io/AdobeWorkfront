using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Requests;

public class CreateProjectRequest
{
    [Display("Project name")]
    public string Name { get; set; } = string.Empty;

    [Display("Object code")]
    public string? ObjCode { get; set; }
    
    [Display("Planned start date")]
    public DateTime? PlannedStartDate { get; set; }
    
    [Display("Planned completion date")]
    public DateTime? PlannedCompletionDate { get; set; }
    
    [Display("Priority")]
    public int? Priority { get; set; }
    
    [Display("Status")]
    public string? Status { get; set; }
    
    [Display("Projected completion date")]
    public DateTime? ProjectedCompletionDate { get; set; }
}