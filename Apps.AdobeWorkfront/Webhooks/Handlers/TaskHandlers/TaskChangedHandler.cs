using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;

public class TaskChangedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext)
{
    protected override string ObjectCode => "TASK";
    
    protected override string EventType => "UPDATE";
}