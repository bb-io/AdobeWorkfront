using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Task;

public class TaskBasicEntity
{
    [JsonProperty("ID")]
    public string TaskId { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonProperty("objCode")]
    public string ObjCode { get; set; } = string.Empty;
    
    [JsonProperty("assignedToID")]
    public string? AssignedToId { get; set; }
    
    [JsonProperty("assignmentsListString")]
    public string? AssignedToNames { get; set; }
}