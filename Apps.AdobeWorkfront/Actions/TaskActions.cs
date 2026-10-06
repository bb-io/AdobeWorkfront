using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Entities.Document;
using Apps.AdobeWorkfront.Models.Entities.Task;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Models.Responses.Task;
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
    private const string TaskFields = Fields.TaskFields;
    
    [Action("Search tasks", Description = "Retrieve a list of tasks based on search criteria")]
    public async Task<SearchTasksResponse> SearchTasks([ActionParameter] SearchTasksRequest request)
    {
        request.Validate();
        
        var apiRequest = new RestRequest("/attask/api/v19.0/task/search");
        var parameters = request.GetFilterQueryParameters();
        apiRequest.ApplyToRequest(parameters);
        
        apiRequest.AddQueryParameter("fields", TaskFields);
        
        var response = await Client.Paginate<TaskFullEntity>(apiRequest);
        return new(response.Select(x => new TaskResponse(x)).ToList());
    }
    
    [Action("Get task", Description = "Retrieve a specific task by its ID")]
    public async Task<TaskWithDocumentsResponse> GetTask([ActionParameter] TaskRequest taskRequest)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/task/{taskRequest.TaskId}");
        apiRequest.AddQueryParameter("fields", TaskFields);
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskWithDocumentsEntity>>(apiRequest);
        var task = response.Data;

        if (task.Documents is null || !task.Documents.Any())
            return new(task);
        
        var documentTasks = task.Documents.Select(async doc =>
        {
            var docRequest = new RestRequest($"/attask/api/v19.0/docu/{doc.DocumentId}");
            docRequest.AddQueryParameter("fields", "name,downloadURL,currentVersion:ext");

            var docResponse = await Client.ExecuteWithErrorHandling<DataWrapperDto<DocumentEntity>>(docRequest);
            var fullDoc = docResponse.Data;

            return fullDoc;
        });

        task.Documents = await Task.WhenAll(documentTasks);
        return new(task);
    }
    
    [Action("Create task", Description = "Create a new task")]
    public async Task<TaskWithDocumentsResponse> CreateTask([ActionParameter] CreateTaskRequest createRequest)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/task", Method.Post)
            .AddQueryParameter("projectID", createRequest.ProjectId)
            .AddQueryParameter("name", createRequest.Name);
        
        if (createRequest.Priority.HasValue)
        {
            apiRequest.AddQueryParameter("priority", createRequest.Priority.Value);
        }
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskBasicEntity>>(apiRequest);
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

        if (updateRequest.PercentComplete.HasValue)
        {
            apiRequest.AddQueryParameter("percentComplete", updateRequest.PercentComplete.Value);
        }
        
        apiRequest.AddQueryParameter("fields", TaskFields);
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskFullEntity>>(apiRequest);
        if (updateRequest.AssigneeIds != null)
        {
            await AssignUsersToTask(response.Data.TaskId, updateRequest.AssigneeIds);
        }
        
        return new(response.Data);
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