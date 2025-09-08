using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront.Webhooks.Handlers.ProjectHandlers;

public class ProjectDeletedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext)
{
    protected override string ObjectCode => "PROJ";
    
    protected override string EventType => "DELETE";
}