using Apps.AdobeWorkfront.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Webhooks.Handlers.ProjectHandlers;

public class ProjectStatusChangedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext)
{
    protected override string ObjectCode => "PROJ";
    
    protected override string EventType => "UPDATE";

    protected override List<FilterDto> Filters => new()
    {
        new FilterDto
        {
            FieldName = "status",
            FieldValue = "",
            Comparison = "changed"
        }
    };
}