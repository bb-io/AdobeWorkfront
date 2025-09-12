using Apps.AdobeWorkfront.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;

public class TaskStatusChangedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext)
{
    protected override string ObjectCode => "TASK";
    
    protected override string EventType => "UPDATE";

    protected override List<FilterDto> Filters =>
    [
        new FilterDto
        {
            FieldName = "status",
            FieldValue = "",
            Comparison = "changed"
        }
    ];
}