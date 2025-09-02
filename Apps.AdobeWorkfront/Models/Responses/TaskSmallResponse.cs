using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class TaskSmallResponse
{
    [Display("Task ID"), JsonProperty("ID")]
    public string TaskId { get; set; } = string.Empty;
    
    [Display("Task name"), JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Task status"), JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
    
    [Display("Object code"), JsonProperty("objCode")]
    public string ObjCode { get; set; } = string.Empty;
    
    [Display("Assigned to ID"), JsonProperty("assignedToID")]
    public string? AssignedToId { get; set; }
    
    [Display("Assigned to name"), JsonProperty("assignmentsListString")]
    public string? AssignedToName { get; set; }
}