using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Tasks")]
public class TaskActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    private const string TaskFields =
        "percentComplete,plannedCompletionDate,plannedStartDate,priority,progressStatus,projectedCompletionDate,projectedStartDate,status,taskNumber,wbs,assignmentsListString,assignedToID,parentID,description";
    
    [Action("Search tasks", Description = "Retrieve a list of tasks based on search criteria")]
    public async Task<SearchTasksResponse> SearchTasks([ActionParameter] SearchTasksRequest request)
    {
        
        var apiRequest = new RestRequest("/attask/api/v19.0/task/search");
        var parameters = request.GetFilterQueryParameters();
        apiRequest.ApplyToRequest(parameters);
        
        apiRequest.AddQueryParameter("fields", TaskFields);
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<List<TaskResponse>>>(apiRequest);
        return new(response.Data);
    }
    
    [Action("Get task", Description = "Retrieve a specific task by its ID")]
    public async Task<TaskResponse> GetTask([ActionParameter] TaskRequest taskRequest)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/task/{taskRequest.TaskId}");
        apiRequest.AddQueryParameter("fields", TaskFields);
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskResponse>>(apiRequest);
        return response.Data;
    }
    
    [Action("Create task", Description = "Create a new task")]
    public async Task<TaskResponse> CreateTask([ActionParameter] CreateTaskRequest createRequest)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/task", Method.Post)
            .AddQueryParameter("projectID", createRequest.ProjectId)
            .AddQueryParameter("name", createRequest.Name);
        
        if (createRequest.Priority.HasValue)
        {
            apiRequest.AddQueryParameter("priority", createRequest.Priority.Value);
        }
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskResponse>>(apiRequest);
        if (createRequest.AssigneeIds != null)
        {
            await AssignUsersToTask(response.Data.TaskId, createRequest.AssigneeIds);
        }
        
        return await GetTask(new() { TaskId = response.Data.TaskId });
    }
    
    [Action("Update task", Description = "Update an existing task with new details")]
    public async Task<TaskResponse> UpdateTask([ActionParameter] UpdateTaskRequest updateRequest)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/task", Method.Put)
            .AddQueryParameter("id", updateRequest.TaskId);
        
        if (!string.IsNullOrEmpty(updateRequest.Name))
        {
            apiRequest.AddQueryParameter("name", updateRequest.Name);
        }
        
        if (!string.IsNullOrEmpty(updateRequest.Status))
        {
            apiRequest.AddQueryParameter("status", updateRequest.Status);
        }
        
        if (updateRequest.Priority.HasValue)
        {
            apiRequest.AddQueryParameter("priority", updateRequest.Priority.Value);
        }
        
        apiRequest.AddQueryParameter("fields", TaskFields);
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskResponse>>(apiRequest);
        if (updateRequest.AssigneeIds != null)
        {
            await AssignUsersToTask(response.Data.TaskId, updateRequest.AssigneeIds);
        }
        
        return response.Data;
    }
    
    [Action("Delete task", Description = "Delete a task by its ID")]
    public async Task DeleteTask([ActionParameter] TaskRequest taskRequest)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/task/{taskRequest.TaskId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
    
    private async Task AssignUsersToTask(string taskId, IEnumerable<string> userIds)
    {
        try
        {
            var apiRequest = new RestRequest($"/attask/api/v19.0/task/{taskId}/assignMultiple", Method.Put)
                .AddJsonBody(new
                {
                    userIDs = userIds,
                    roleIDs = Array.Empty<string>()
                });

            await Client.ExecuteWithErrorHandling(apiRequest);
        }
        catch (Exception e)
        {
            throw new PluginApplicationException($"Could not assign users to task {taskId}: {e.Message}");
        }
    }
}