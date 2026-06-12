using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.TaskHandlers;
using Apps.AdobeWorkfront.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Tasks")]
public class TaskWebhookList(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On task status changed", typeof(TaskStatusChangedHandler), Description = "Triggers when a task's status changes")]
    public Task<WebhookResponse<TaskResponse>> OnTaskStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] TaskStatusOptionalRequest taskStatusOptionalRequest,
        [WebhookParameter] TaskOptionalRequest taskOptionalRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => (taskStatusOptionalRequest.TaskStatus == null || payload.NewState.Status.Equals(taskStatusOptionalRequest.TaskStatus, StringComparison.OrdinalIgnoreCase)) &&
                   (taskOptionalRequest.TaskId == null || payload.NewState.TaskId.Equals(taskOptionalRequest.TaskId, StringComparison.OrdinalIgnoreCase)));

    [Webhook("On task changed", typeof(TaskChangedHandler), Description = "Triggers when any property of a task changes")]
    public async Task<WebhookResponse<TaskResponse>> OnTaskChanged(WebhookRequest webhookRequest,
        [WebhookParameter] TaskOptionalRequest taskOptionalRequest,
        [WebhookParameter] OnTaskChangedRequest taskChangedRequest) 
    {
        var webhookResponse = await HandleWebhook<TaskResponse>(webhookRequest, payload => 
        {
            var task = payload.NewState;

            if (!string.IsNullOrWhiteSpace(taskOptionalRequest.TaskId) && 
                !task.TaskId.Equals(taskOptionalRequest.TaskId, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.IsNullOrWhiteSpace(taskChangedRequest.TaskNameContains) && 
                !(task.Name?.Contains(taskChangedRequest.TaskNameContains, StringComparison.OrdinalIgnoreCase) ?? false))
                return false;

            return true;
        });

        if (webhookResponse.ReceivedWebhookRequestType != WebhookRequestType.Default || webhookResponse.Result == null)
            return webhookResponse;
        
        var apiRequest = new RestRequest($"/attask/api/v19.0/task/{webhookResponse.Result.TaskId}")
            .AddQueryParameter("fields", Fields.TaskFields);
        
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<TaskResponse>>(apiRequest);
        webhookResponse.Result.ProjectName = response.Data.ProjectName;

        return webhookResponse;
    }
    
    [Webhook("On task created", typeof(TaskCreatedHandler), Description = "Triggers when a new task is created")]
    public Task<WebhookResponse<TaskResponse>> OnTaskCreated(WebhookRequest webhookRequest) => HandleWebhook<TaskResponse>(webhookRequest, 
        payload => true);
}