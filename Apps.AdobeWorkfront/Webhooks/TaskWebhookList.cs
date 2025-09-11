using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Tasks")]
public class TaskWebhookList(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On task status changed", typeof(TaskStatusChangedHandler), Description = "Triggers when a task's status changes")]
    public Task<WebhookResponse<TaskResponse>> OnTaskStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] TaskStatusOptionalRequest taskStatusOptionalRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => taskStatusOptionalRequest.TaskStatus == null || payload.NewState.Status.Equals(taskStatusOptionalRequest.TaskStatus, StringComparison.OrdinalIgnoreCase));

    [Webhook("On task changed", typeof(TaskChangedHandler), Description = "Triggers when any property of a task changes")]
    public Task<WebhookResponse<TaskResponse>> OnTaskChanged(WebhookRequest webhookRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => true);

    [Webhook("On task created", typeof(TaskCreatedHandler), Description = "Triggers when a new task is created")]
    public Task<WebhookResponse<TaskResponse>> OnTaskCreated(WebhookRequest webhookRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => true);
}