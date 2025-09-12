using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Webhooks.Handlers.ProjectHandlers;

public class ProjectChangedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext)
{
    protected override string ObjectCode => "PROJ";
    
    protected override string EventType => "UPDATE";
}