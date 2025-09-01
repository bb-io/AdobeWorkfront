using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Projects")]
public class ProjectActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Search projects", Description = "Retrieve a list of projects based on search criteria")]
    public async Task<SearchProjectsResponse> SearchProjects([ActionParameter] SearchProjectsRequest request)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/project/search");
        var parameters = request.GetFilterQueryParameters();
        apiRequest.ApplyToRequest(parameters);
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<List<ProjectResponse>>>(apiRequest);
        return new(response.Data);
    }
    
    [Action("Get project", Description = "Retrieve details of a specific project by its ID")]
    public async Task<ProjectResponse> GetProject([ActionParameter] ProjectRequest projectRequest)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/project/{projectRequest.ProjectId}");
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ProjectResponse>>(apiRequest);
        return response.Data;
    }
    
    [Action("Create project", Description = "Create a new project with the provided details")]
    public async Task<ProjectResponse> CreateProject([ActionParameter] CreateProjectRequest createRequest)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/project", Method.Post)
            .AddQueryParameter("name", createRequest.Name);
        
        if(!string.IsNullOrEmpty(createRequest.ObjCode))
        {
            apiRequest.AddQueryParameter("objCode", createRequest.ObjCode);
        }
        
        if(createRequest.PlannedStartDate.HasValue)
        {
            apiRequest.AddQueryParameter("plannedStartDate",
                createRequest.PlannedStartDate.Value.ToString(DateTimeFormats.Fmt));
        }
        
        if(createRequest.PlannedCompletionDate.HasValue)
        {
            apiRequest.AddQueryParameter("plannedCompletionDate",
                createRequest.PlannedCompletionDate.Value.ToString(DateTimeFormats.Fmt));
        }
        
        if(createRequest.Priority.HasValue)
        {
            apiRequest.AddQueryParameter("priority", createRequest.Priority.Value.ToString());
        }
        
        if(!string.IsNullOrEmpty(createRequest.Status))
        {
            apiRequest.AddQueryParameter("status", createRequest.Status);
        }
        
        if(createRequest.ProjectedCompletionDate.HasValue)
        {
            apiRequest.AddQueryParameter("projectedCompletionDate",
                createRequest.ProjectedCompletionDate.Value.ToString(DateTimeFormats.Fmt));
        }
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ProjectResponse>>(apiRequest);
        return response.Data;
    }
    
    [Action("Delete project", Description = "Delete a specific project by its ID")]
    public async Task DeleteProject([ActionParameter] ProjectRequest projectRequest)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/project/{projectRequest.ProjectId}", Method.Delete);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
}