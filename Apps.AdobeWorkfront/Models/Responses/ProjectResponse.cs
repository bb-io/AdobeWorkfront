using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class ProjectResponse
{
    [JsonProperty("ID"), Display("Project ID")]
    public string ProjectId { get; set; } = string.Empty;
    
    [JsonProperty("name"), Display("Project name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("status"), Display("Project status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonProperty("objCode"), Display("Object code")]
    public string ObjCode { get; set; } = string.Empty;
    
    [JsonProperty("percentComplete"), Display("Percent complete")]
    public double PercentComplete { get; set; }
    
    [JsonProperty("plannedCompletionDate"), Display("Planned completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime PlannedCompletionDate { get; set; }
    
    [JsonProperty("plannedStartDate"), Display("Planned start date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime PlannedStartDate { get; set; }
    
    [JsonProperty("priority")]
    public int Priority { get; set; }
    
    [JsonProperty("projectedCompletionDate"), Display("Projected completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime ProjectedCompletionDate { get; set; }
}