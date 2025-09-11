using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Projects")]
public class ProjectWebhookList(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On project status changed", typeof(ProjectStatusChangedHandler), Description = "Triggers when a project's status changes")]
    public Task<WebhookResponse<ProjectResponse>> OnProjectStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectStatusOptionalRequest projectStatusOptionalRequest) => HandleWebhook<ProjectResponse>(webhookRequest, 
        payload => projectStatusOptionalRequest.ProjectStatus == null || payload.NewState.Status.Equals(projectStatusOptionalRequest.ProjectStatus, StringComparison.OrdinalIgnoreCase));

    [Webhook("On project changed", typeof(ProjectChangedHandler), Description = "Triggers when any property of a project changes")]
    public Task<WebhookResponse<ProjectResponse>> OnProjectChanged(WebhookRequest webhookRequest) => HandleWebhook<ProjectResponse>(webhookRequest, 
        payload => true);

    [Webhook("On project created", typeof(ProjectCreatedHandler), Description = "Triggers when a new project is created")]
    public Task<WebhookResponse<ProjectResponse>> OnProjectCreated(WebhookRequest webhookRequest) => HandleWebhook<ProjectResponse>(webhookRequest, 
        payload => true);
}