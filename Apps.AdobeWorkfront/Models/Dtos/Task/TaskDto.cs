using Apps.AdobeWorkfront.Models.Dtos.Document;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Dtos.Task;

public record TaskDto
{
    public TaskDto(GetTaskResponse response)
    {
        TaskId = response.TaskId;
        Name = response.Name;
        Status = response.Status;
        ObjCode = response.ObjCode;
        AssignedToId = response.AssignedToId;
        AssignedToNames = response.AssignedToNames;
        ProjectId = response.ProjectId;
        ProjectName = response.ProjectName;
        ProgressStatus = response.ProgressStatus;
        PercentComplete = response.PercentComplete;
        ParentId = response.ParentId;
        Description = response.Description;
        PlannedCompletionDate = response.PlannedCompletionDate;
        PlannedStartDate = response.PlannedStartDate;
        Priority = response.Priority;
        ProjectedCompletionDate = response.ProjectedCompletionDate;
        ProjectedStartDate = response.ProjectedStartDate;
        TaskNumber = response.TaskNumber;
        Wbs = response.Wbs;
        Documents = response.Documents?.Select(x => new DocumentDto(x));
    }
    
    [Display("Task ID")]
    public string TaskId { get; set; }
    
    [Display("Task name")]
    public string Name { get; set; }
    
    [Display("Task status")]
    public string Status { get; set; }
    
    [Display("Object code")]
    public string ObjCode { get; set; }
    
    [Display("Assigned to ID")]
    public string? AssignedToId { get; set; }
    
    [Display("Assigned to names")]
    public string? AssignedToNames { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Project name")]
    public string ProjectName { get; set; }
    
    [Display("Progress status")]
    public string ProgressStatus { get; set; }
    
    [Display("Percent complete")]
    public double PercentComplete { get; set; }
    
    [Display("Parent ID")]
    public string ParentId { get; set; }

    [Display("Description")]
    public string Description { get; set; }
    
    [Display("Planned completion date")]
    public DateTime? PlannedCompletionDate { get; set; }
    
    [Display("Planned start date")]
    public DateTime? PlannedStartDate { get; set; }
    
    [Display("Priority")]
    public int Priority { get; set; }
    
    [Display("Projected completion date")]
    public DateTime? ProjectedCompletionDate { get; set; }
    
    [Display("Projected start date")]
    public DateTime? ProjectedStartDate { get; set; }
    
    [Display("Task number")]
    public int TaskNumber { get; set; }
    
    [Display("WBS")]
    public string Wbs { get; set; }
    
    [Display("Documents")]
    public IEnumerable<DocumentDto>? Documents { get; set; }
}