using Apps.AdobeWorkfront.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Handlers;

public class ParentDataHandler(InvocationContext invocationContext, [ActionParameter] UploadFileRequest uploadFileRequest) 
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(uploadFileRequest.ParentType))
        {
            throw new ArgumentNullException("Parent type must be specified to get parent IDs.");
        }
        
        IAsyncDataSourceItemHandler handler = uploadFileRequest.ParentType switch
        {
            "PROJ" => new ProjectDataHandler(InvocationContext),
            "TASK" => new TaskDataHandler(InvocationContext),
            _ => throw new ArgumentOutOfRangeException(nameof(uploadFileRequest.ParentType), $"Unsupported parent type: {uploadFileRequest.ParentType}")
        };

        return handler.GetDataAsync(context, cancellationToken);
    }
}