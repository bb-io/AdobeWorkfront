using Apps.AdobeWorkfront.Api;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Webhooks.Bridge;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.AdobeWorkfront.Webhooks.Handlers;

public abstract class BaseWebhookHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IWebhookEventHandler
{
    private const string AppName = "adobeworkfront";
    
    private readonly BridgeService _bridgeService = new($"{invocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/{AppName}");
    
    protected abstract string ObjectCode { get; }
    
    protected abstract string EventType { get; }

    protected virtual List<FilterDto> Filters => new();

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var apiClient = new Client(authenticationCredentialsProvider.ToList());
        var existingWebhook = await GetNeededWebhookSubscription(apiClient);
        if (existingWebhook == null)
        {
            var webhookRequest = new CreateWebhookRequestDto
            {
                ObjCode = ObjectCode,
                EventType = EventType,
                Url = values["payloadUrl"],
                AuthToken = BuildAuthToken(),
                Filters = Filters
            };

            var subscription = await CreateWebhookSubscription(apiClient, webhookRequest);
            await _bridgeService.Subscribe(FullEventType, subscription.Id, values["payloadUrl"]);
        }
        else
        {
            await _bridgeService.Subscribe(FullEventType, existingWebhook.Id, values["payloadUrl"]);
        }
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var apiClient = new Client(authenticationCredentialsProvider.ToList());
        var existingWebhook = await GetNeededWebhookSubscription(apiClient);
        if (existingWebhook != null)
        {
            await _bridgeService.Unsubscribe(FullEventType, existingWebhook.Id, values["payloadUrl"]);
            var isAnySubscriberExist = await _bridgeService.IsAnySubscriberExist(FullEventType, existingWebhook.Id);
            if (!isAnySubscriberExist)
            {
                await DeleteWebhookSubscription(apiClient, existingWebhook.Id);
            }
        }
    }
    
    private string FullEventType => $"{ObjectCode}_{EventType}";
    
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
    
    private async Task<WebhookSubscriptionDto?> GetNeededWebhookSubscription(Client apiClient)
    {
        var subscriptions = await GetWebhookSubscriptions(apiClient);
        var webhook = subscriptions.Subscriptions.FirstOrDefault(w => 
            w.ObjCode == ObjectCode && 
            w.EventType == EventType && 
            w.Filters.Count == Filters.Count &&
            (Filters.Count == 0 || Filters.All(f => w.Filters.Any(wf => wf.FieldName == f.FieldName && wf.Comparison == f.Comparison))) &&
            w.Url.StartsWith(InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')));
        
        return webhook;
    }

    private string BuildAuthToken() => $"{InvocationContext.Tenant.Id}:{InvocationContext.Tenant.Name}";
}