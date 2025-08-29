using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Projects")]
public class ProjectActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Search projects", Description = "Retrieve a list of projects based on search criteria")]
    public async Task<SearchProjectsResponse> SearchProjects()
    {
        var request = new RestRequest("/attask/api/v19.0/project/search");
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ProjectResponse>>(request);
        return new(response.Data);
    }
}