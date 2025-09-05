using Apps.AdobeWorkfront.Api;
using Apps.AdobeWorkfront.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.AdobeWorkfront.Webhooks.Handlers;

public abstract class BaseWebhookHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IWebhookEventHandler
{
    protected abstract string ObjectCode { get; }
    
    protected abstract string EventType { get; }

    protected virtual List<FilterDto> Filters => new();

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var webhookRequest = new CreateWebhookRequestDto
        {
            ObjCode = ObjectCode,
            EventType = EventType,
            Url = values["payloadUrl"],
            AuthToken = "blackbird-webhook-token",
            Filters = Filters
        };

        var apiClient = new Client(authenticationCredentialsProvider.ToList());
        await CreateWebhookSubscription(apiClient, webhookRequest);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var apiClient = new Client(authenticationCredentialsProvider.ToList());
        var subscriptions = await GetWebhookSubscriptions(apiClient);
        var webhook = subscriptions.Subscriptions.FirstOrDefault(w => 
            w.Url == values["payloadUrl"] && 
            w.ObjCode == ObjectCode && 
            w.EventType == EventType);

        if (webhook != null)
        {
            await DeleteWebhookSubscription(apiClient, webhook.Id);
        }
    }
    
    private async Task<WebhookSubscriptionsResponseDto> GetWebhookSubscriptions(Client apiClient)
    {
        var request = new RestRequest("/attask/eventsubscription/api/v1/subscriptions");
        return await apiClient.ExecuteWithErrorHandling<WebhookSubscriptionsResponseDto>(request);
    }

    private async Task<CreateWebhookResponseDto> CreateWebhookSubscription(Client apiClient, CreateWebhookRequestDto webhookRequest)
    {
        var request = new RestRequest("/attask/eventsubscription/api/v1/subscriptions", Method.Post)
            .AddJsonBody(webhookRequest);
        return await apiClient.ExecuteWithErrorHandling<CreateWebhookResponseDto>(request);
    }

    private async Task DeleteWebhookSubscription(Client apiClient, string subscriptionId)
    {
        var request = new RestRequest($"/attask/eventsubscription/api/v1/subscriptions/{subscriptionId}", Method.Delete);
        await apiClient.ExecuteWithErrorHandling(request);
    }
}