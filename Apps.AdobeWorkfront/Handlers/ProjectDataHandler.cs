using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Handlers;

public class ProjectDataHandler(InvocationContext invocationContext) : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var apiRequest = new RestRequest("/attask/api/v19.0/project/search");
        if (!string.IsNullOrEmpty(context.SearchString))
        {
            apiRequest.AddQueryParameter("name", context.SearchString);
            apiRequest.AddQueryParameter("name_Mod", "contains");
        }
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<List<ProjectResponse>>>(apiRequest);
        return response.Data.Select(x => new DataSourceItem(x.ProjectId, x.Name));
    }
}