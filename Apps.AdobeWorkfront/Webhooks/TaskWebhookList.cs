using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;
using Apps.AdobeWorkfront.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Tasks")]
public class TaskWebhookList(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Webhook("On task status changed", typeof(TaskStatusChangedHandler), Description = "Triggers when a task's status changes")]
    public Task<WebhookResponse<TaskResponse>> OnTaskStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] TaskStatusOptionalRequest taskStatusOptionalRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => taskStatusOptionalRequest.TaskStatus == null || payload.NewState.Status.Equals(taskStatusOptionalRequest.TaskStatus, StringComparison.OrdinalIgnoreCase));

    private async Task<WebhookResponse<T>> HandleWebhook<T>(WebhookRequest webhookRequest, Func<WebhookPayload<T>, bool> triggerFlight) where T : BaseResponse
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
            return new WebhookResponse<T>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = payload.NewState
            };
        }
        
        var task = await GetTask<T>(payload.NewState.GetId());
        return new WebhookResponse<T>
        {
            ReceivedWebhookRequestType = WebhookRequestType.Preflight,
            Result = task
        };
    }

    private async Task<T> GetTask<T>(string taskId)
    {
        try
        {
            var apiRequest = new RestRequest($"/attask/api/v19.0/task/{taskId}");
            apiRequest.AddQueryParameter("fields", Fields.TaskFields);
            var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<T>>(apiRequest);
            return response.Data;
        }
        catch (Exception e)
        {
            throw new Exception($"[Workfront] Unable to retrieve task with ID {taskId}.", e);
        }
    }
}