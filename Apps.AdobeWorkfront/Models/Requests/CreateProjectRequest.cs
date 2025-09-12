using Apps.AdobeWorkfront.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.AdobeWorkfront.Models.Requests;

public class CreateProjectRequest
{
    [Display("Project name")]
    public string Name { get; set; } = string.Empty;

    [Display("Object code")]
    public string? ObjCode { get; set; }
    
    [Display("Planned start date")]
    public DateTime? PlannedStartDate { get; set; }
    
    [Display("Priority")]
    public int? Priority { get; set; }
    
    [Display("Status"), StaticDataSource(typeof(ProjectStatusDataHandler))]
    public string? Status { get; set; }
}