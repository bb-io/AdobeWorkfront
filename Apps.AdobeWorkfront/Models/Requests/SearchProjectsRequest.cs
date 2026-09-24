using Apps.AdobeWorkfront.Handlers.Static;
using Apps.AdobeWorkfront.Models.Entities;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.AdobeWorkfront.Models.Requests;

public class SearchProjectsRequest
{
    [Display("Project name")]
    public string? Name { get; set; }
    
    [Display("Project status"), StaticDataSource(typeof(ProjectStatusDataHandler))]
    public string? Status { get; set; }

    [Display("Planned completion date from")]
    public DateTime? PlannedCompletionDateFrom { get; set; }
    
    [Display("Planned completion date to")]
    public DateTime? PlannedCompletionDateTo { get; set; }
    
    [Display("Planned start date from")]
    public DateTime? PlannedStartDateFrom { get; set; }
    
    [Display("Planned start date to")]
    public DateTime? PlannedStartDateTo { get; set; }
    
    [Display("Project completion date from")]
    public DateTime? ProjectedCompletionDateFrom { get; set; }
    
    [Display("Project completion date to")]
    public DateTime? ProjectedCompletionDateTo { get; set; }

    public List<QueryParameter> GetFilterQueryParameters()
    {
        return new List<QueryParameter>()
            .AddIfFilter("name", Name)
            .AddIfFilter("status", Status)
            .AddRangeFilter("plannedStartDate", PlannedStartDateFrom, PlannedStartDateTo)
            .AddRangeFilter("plannedCompletionDate", PlannedCompletionDateFrom, PlannedCompletionDateTo)
            .AddRangeFilter("projectedCompletionDate", ProjectedCompletionDateFrom, ProjectedCompletionDateTo);
    }
}