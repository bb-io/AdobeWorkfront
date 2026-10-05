using Apps.AdobeWorkfront.Handlers.Static;
using Apps.AdobeWorkfront.Models.Entities;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.AdobeWorkfront.Models.Requests;

public class SearchTasksRequest
{
    [Display("Task name equals")]
    public string? Name { get; set; }

    [Display("Task name contains (case-sensitive)")]
    public string? NameContainsSensitive { get; set; }

    [Display("Task name contains (case-insensitive)")]
    public string? NameContainsInsensitive { get; set; }
    
    [Display("Task status"), StaticDataSource(typeof(TaskStatusDataHandler))]
    public string? Status { get; set; }
    
    [Display("Progress status"), StaticDataSource(typeof(ProgressStatusDataHandler))]
    public string? ProgressStatus { get; set; }

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

    [Display("Entry date from")]
    public DateTime? EntryDateFrom { get; set; }

    [Display("Entry date to")]
    public DateTime? EntryDateTo { get; set; }

    public List<QueryParameter> GetFilterQueryParameters()
    {
        return new List<QueryParameter>()
            .AddIfFilter("name", Name)
            .AddContainsFilter("name", NameContainsSensitive, caseSensitive: true)
            .AddContainsFilter("name", NameContainsInsensitive, caseSensitive: false)
            .AddIfFilter("status", Status)
            .AddIfFilter("progressStatus", ProgressStatus)
            .AddRangeFilter("plannedStartDate", PlannedStartDateFrom, PlannedStartDateTo)
            .AddRangeFilter("plannedCompletionDate", PlannedCompletionDateFrom, PlannedCompletionDateTo)
            .AddRangeFilter("projectedCompletionDate", ProjectedCompletionDateFrom, ProjectedCompletionDateTo)
            .AddRangeFilter("entryDate", EntryDateFrom, EntryDateTo);
    }
}