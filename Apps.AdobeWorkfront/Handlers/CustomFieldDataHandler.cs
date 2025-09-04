using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Handlers;

public class CustomFieldDataHandler(InvocationContext invocationContext, [ActionParameter] ParentRequest uploadFileRequest) 
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(uploadFileRequest.ParentType))
        {
            throw new Exception("Parent type must be specified to get parent IDs.");
        }
        
        if (string.IsNullOrEmpty(uploadFileRequest.ParentId))
        {
            throw new Exception("Parent ID must be specified to get custom fields.");
        }

        var apiParentType = uploadFileRequest.GetParentTypeForApi();
        var requestUrl = $"/attask/api/v19.0/{apiParentType}/{uploadFileRequest.ParentId}";
        var apiRequest = new RestRequest(requestUrl)
            .AddQueryParameter("fields", "parameterValues:*");

        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ObjectWithCustomFieldsDto>>(apiRequest);
        return response.Data.CustomFields
            .Where(x => context.SearchString == null || x.Key.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(cf => new DataSourceItem(cf.Key, cf.Key))
            .ToList();
    }
}