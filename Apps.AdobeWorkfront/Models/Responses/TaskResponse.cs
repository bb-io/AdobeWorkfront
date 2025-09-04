using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class TaskResponse : TaskSmallResponse
{    
    [JsonProperty("progressStatus"), Display("Progress status")]
    public string ProgressStatus { get; set; } = string.Empty;
    
    [JsonProperty("percentComplete"), Display("Percent complete")]
    public double PercentComplete { get; set; }
    
    [JsonProperty("parentID"), Display("Parent ID")]
    public string ParentId { get; set; } = string.Empty;

    [JsonProperty("description"), Display("Description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonProperty("plannedCompletionDate"), Display("Planned completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime PlannedCompletionDate { get; set; }
    
    [JsonProperty("plannedStartDate"), Display("Planned start date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime PlannedStartDate { get; set; }
    
    [JsonProperty("priority")]
    public int Priority { get; set; }
    
    [JsonProperty("projectedCompletionDate"), Display("Projected completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime ProjectedCompletionDate { get; set; }
    
    [JsonProperty("projectedStartDate"), Display("Projected start date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime ProjectedStartDate { get; set; }
    
    [JsonProperty("taskNumber"), Display("Task number")]
    public int TaskNumber { get; set; }
    
    [JsonProperty("wbs"), Display("WBS")]
    public string Wbs { get; set; } = string.Empty;
}