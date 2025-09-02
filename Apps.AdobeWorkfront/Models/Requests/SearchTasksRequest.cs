using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Handlers.Static;
using Apps.AdobeWorkfront.Models.Entities;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.AdobeWorkfront.Models.Requests;

public class SearchTasksRequest
{
    [Display("Task name")]
    public string? Name { get; set; }
    
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

    public List<QueryParameter> GetFilterQueryParameters()
    {
        var result = new List<QueryParameter>();

        void AddIf(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                result.Add(new QueryParameter(key, value!));
            }
        }

        void AddRangeFilter(string field, DateTimeOffset? from, DateTimeOffset? to)
        {
            if (from is { } f)
            {
                result.Add(new QueryParameter(field, f.ToString(DateTimeFormats.Fmt)));
                result.Add(new QueryParameter($"{field}_Mod", "gte"));
            }

            if (to is { } t)
            {
                result.Add(new QueryParameter(field, t.ToString(DateTimeFormats.Fmt)));
                result.Add(new QueryParameter($"{field}_Mod", "lte"));
            }
        }

        AddIf("name", Name);
        AddIf("status", Status);
        AddIf("progressStatus", ProgressStatus);

        AddRangeFilter("plannedStartDate", PlannedStartDateFrom, PlannedStartDateTo);
        AddRangeFilter("plannedCompletionDate", PlannedCompletionDateFrom, PlannedCompletionDateTo);
        AddRangeFilter("projectedCompletionDate",ProjectedCompletionDateFrom, ProjectedCompletionDateTo);

        return result;
    }
}