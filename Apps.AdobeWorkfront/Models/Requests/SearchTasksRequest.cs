using Apps.AdobeWorkfront.Handlers;
using Apps.AdobeWorkfront.Handlers.Static;
using Apps.AdobeWorkfront.Models.Entities;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.AdobeWorkfront.Models.Requests;

public class SearchTasksRequest
{
    [Display("Task name equals")]
    public string? Name { get; set; }

    [Display("Task name contains (case-sensitive)")]
    public string? NameContainsSensitive { get; set; }

    [Display("Task name contains (case-insensitive)")]
    public string? NameContainsInsensitive { get; set; }

    [Display("Parent task name equals")]
    public string? ParentName { get; set; }

    [Display("Parent task name contains (case-sensitive)")]
    public string? ParentNameContainsSensitive { get; set; }

    [Display("Parent task name contains (case-insensitive)")]
    public string? ParentNameContainsInsensitive { get; set; }

    [Display("Parent task ID"), DataSource(typeof(TaskDataHandler))]
    public string? ParentId { get; set; }

    [Display("Project ID"), DataSource(typeof(ProjectDataHandler))]
    public string? ProjectId { get; set; }
    
    [Display("Task status"), StaticDataSource(typeof(TaskStatusDataHandler))]
    public string? Status { get; set; }
    
    [Display("Progress status"), StaticDataSource(typeof(ProgressStatusDataHandler))]
    public string? ProgressStatus { get; set; }

    [Display("Entered by user ID"), DataSource(typeof(UserDataHandler))]
    public string? EnteredByUserId { get; set; }

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

    [Display("Percent complete from")]
    public int? PercentCompleteFrom { get; set; }

    [Display("Percent complete to")]
    public int? PercentCompleteTo { get; set; }

    [Display("Raw query", Description = "Extra filters not covered by the inputs above, e.g. priority=2&priority_Mod=gte")]
    public string? RawQuery { get; set; }

    public void Validate()
    {
        if (PercentCompleteFrom.HasValue && PercentCompleteTo.HasValue && PercentCompleteFrom > PercentCompleteTo)
            throw new PluginMisconfigurationException("'Percent complete from' value can't be more than 'Percent complete to'");
        
        int taskNameInputs = new[] { Name, NameContainsInsensitive, NameContainsSensitive }
            .Count(x => !string.IsNullOrWhiteSpace(x));
        if (taskNameInputs > 1)
            throw new PluginMisconfigurationException("Only one Task name input is allowed");
        
        int parentNameInputs = new[] { ParentId, ParentName, ParentNameContainsInsensitive, ParentNameContainsSensitive }
            .Count(x => !string.IsNullOrWhiteSpace(x));
        if (parentNameInputs > 1)
            throw new PluginMisconfigurationException("Only one Parent task input is allowed");
    }

    public List<QueryParameter> GetFilterQueryParameters()
    {
        return new List<QueryParameter>()
            .AddEqualsFilter("name", Name)
            .AddContainsFilter("name", NameContainsSensitive, caseSensitive: true)
            .AddContainsFilter("name", NameContainsInsensitive, caseSensitive: false)
            .AddEqualsFilter("parentID", ParentId)
            .AddEqualsFilter("parent:name", ParentName)
            .AddContainsFilter("parent:name", ParentNameContainsSensitive, caseSensitive: true)
            .AddContainsFilter("parent:name", ParentNameContainsInsensitive, caseSensitive: false)
            .AddEqualsFilter("projectID", ProjectId)
            .AddEqualsFilter("status", Status)
            .AddEqualsFilter("progressStatus", ProgressStatus)
            .AddEqualsFilter("enteredByID", EnteredByUserId)
            .AddRangeFilter("plannedStartDate", PlannedStartDateFrom, PlannedStartDateTo)
            .AddRangeFilter("plannedCompletionDate", PlannedCompletionDateFrom, PlannedCompletionDateTo)
            .AddRangeFilter("projectedCompletionDate", ProjectedCompletionDateFrom, ProjectedCompletionDateTo)
            .AddRangeFilter("entryDate", EntryDateFrom, EntryDateTo)
            .AddRangeFilter("percentComplete", PercentCompleteFrom, PercentCompleteTo)
            .AddRawQuery(RawQuery);
    }
}