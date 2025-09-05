using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;
using Apps.AdobeWorkfront.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Tasks")]
public class TaskWebhookList(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Webhook("On task status changed", typeof(TaskStatusChangedHandler), Description = "Triggers when a task's status changes")]
    public Task<WebhookResponse<TaskResponse>> OnTaskStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] TaskStatusOptionalRequest taskStatusOptionalRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => taskStatusOptionalRequest.TaskStatus == null || payload.NewState.Status.Equals(taskStatusOptionalRequest.TaskStatus, StringComparison.OrdinalIgnoreCase));

    private Task<WebhookResponse<T>> HandleWebhook<T>(WebhookRequest webhookRequest, Func<WebhookPayload<T>, bool> triggerFlight) where T : class
    {
        var body = webhookRequest.Body.ToString();
        if (string.IsNullOrEmpty(body))
        {
            throw new ArgumentException("[Workfront] Incoming webhook body is null or empty.");
        }

        var payload = JsonConvert.DeserializeObject<WebhookPayload<T>>(body);
        if (payload == null)
        {
            throw new ArgumentException("[Workfront] Unable to deserialize incoming webhook body.");
        }

        if (triggerFlight.Invoke(payload) == false)
        {
            return Task.FromResult(new WebhookResponse<T>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = payload.NewState
            });
        }
        
        return Task.FromResult(new WebhookResponse<T>
        {
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = payload.NewState
        });
    }
}