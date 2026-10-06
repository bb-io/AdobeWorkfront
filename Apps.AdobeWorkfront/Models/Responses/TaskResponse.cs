using Apps.AdobeWorkfront.Models.Entities.Task;
using Apps.AdobeWorkfront.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class TaskResponse : TaskSmallResponse
{
    public TaskResponse() { }
    
    public TaskResponse(TaskFullEntity entity)
    {
        TaskId = entity.TaskId;
        Name = entity.Name;
        Status = entity.Status;
        ObjCode = entity.ObjCode;
        AssignedToId = entity.AssignedToId;
        AssignedToNames = entity.AssignedToNames;
        ProjectId = entity.ProjectId;
        ProjectName = entity.Project?.Name ?? string.Empty;
        ProgressStatus = entity.ProgressStatus;
        PercentComplete = entity.PercentComplete;
        ParentId = entity.ParentId;
        ParentName = entity.Parent?.Name;
        Description = entity.Description;
        PlannedCompletionDate = entity.PlannedCompletionDate;
        PlannedStartDate = entity.PlannedStartDate;
        Priority = entity.Priority;
        ProjectedCompletionDate = entity.ProjectedCompletionDate;
        ProjectedStartDate = entity.ProjectedStartDate;
        TaskNumber = entity.TaskNumber;
        Wbs = entity.Wbs;
        EnteredByUserId = entity.EnteredByUserId;
        EnteredByUserName = entity.EnteredByUser?.Name;
        EnteredByUserEmail = entity.EnteredByUser?.Email;
    }
    
    [JsonProperty("projectID"), Display("Project ID")]
    public string ProjectId { get; set; }
    
    [JsonProperty("project"), Display("Project name"), JsonConverter(typeof(WorkfrontProjectNameConverter))]
    public string ProjectName { get; set; }
    
    [JsonProperty("progressStatus"), Display("Progress status")]
    public string ProgressStatus { get; set; }
    
    [JsonProperty("percentComplete"), Display("Percent complete")]
    public double PercentComplete { get; set; }
    
    [JsonProperty("parentID"), Display("Parent ID")]
    public string? ParentId { get; set; }
    
    [Display("Parent name")]
    public string? ParentName { get; set; }

    [JsonProperty("description"), Display("Description")]
    public string Description { get; set; }

    [JsonProperty("plannedCompletionDate"), Display("Planned completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? PlannedCompletionDate { get; set; }
    
    [JsonProperty("plannedStartDate"), Display("Planned start date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? PlannedStartDate { get; set; }
    
    [JsonProperty("priority")]
    public int Priority { get; set; }
    
    [JsonProperty("projectedCompletionDate"), Display("Projected completion date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? ProjectedCompletionDate { get; set; }
    
    [JsonProperty("projectedStartDate"), Display("Projected start date"), JsonConverter(typeof(WorkfrontDateTimeConverter))]
    public DateTime? ProjectedStartDate { get; set; }
    
    [JsonProperty("taskNumber"), Display("Task number")]
    public int TaskNumber { get; set; }
    
    [JsonProperty("wbs"), Display("WBS")]
    public string Wbs { get; set; }

    [JsonProperty("enteredByID"), Display("Entered by user ID")]
    public string? EnteredByUserId { get; set; }

    [Display("Entered by user name")]
    public string? EnteredByUserName { get; set; }

    [Display("Entered by user email")]
    public string? EnteredByUserEmail { get; set; }
}