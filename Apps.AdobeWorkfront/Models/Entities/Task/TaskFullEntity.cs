using Apps.AdobeWorkfront.Models.Entities.Project;
using Apps.AdobeWorkfront.Models.Entities.User;
using Apps.AdobeWorkfront.Utils.Converters;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Task;

public class TaskFullEntity : TaskBasicEntity
{
    [JsonProperty("projectID")]
    public string ProjectId { get; set; } = string.Empty;

    [JsonProperty("project")]
    public ProjectEntity Project { get; set; } = null!;
    
    [JsonProperty("progressStatus")]
    public string ProgressStatus { get; set; } = string.Empty;
    
    [JsonProperty("percentComplete")]
    public double PercentComplete { get; set; }
    
    [JsonProperty("parentID")]
    public string? ParentId { get; set; }

    [JsonProperty("parent")]
    public TaskBasicEntity? Parent { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonProperty("plannedCompletionDate"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? PlannedCompletionDate { get; set; }
    
    [JsonProperty("plannedStartDate"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? PlannedStartDate { get; set; }
    
    [JsonProperty("priority")]
    public int Priority { get; set; }
    
    [JsonProperty("projectedCompletionDate"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? ProjectedCompletionDate { get; set; }
    
    [JsonProperty("projectedStartDate"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? ProjectedStartDate { get; set; }
    
    [JsonProperty("taskNumber")]
    public int TaskNumber { get; set; }
    
    [JsonProperty("wbs")]
    public string Wbs { get; set; } = string.Empty;

    [JsonProperty("enteredByID")]
    public string? EnteredByUserId { get; set; }

    [JsonProperty("enteredBy")]
    public UserEntity? EnteredByUser { get; set; }
    
    [JsonProperty("parameterValues")]
    public Dictionary<string, object?>? CustomFields { get; set; }
}