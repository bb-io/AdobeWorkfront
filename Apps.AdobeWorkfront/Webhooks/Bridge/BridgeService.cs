using Apps.AdobeWorkfront.Webhooks.Bridge.Models;
using RestSharp;

namespace Apps.AdobeWorkfront.Webhooks.Bridge;

public class BridgeService(string bridgeServiceUrl)
{
    private readonly RestClient _client = new(bridgeServiceUrl);

    public async Task Subscribe(string @event, string subscriptionId, string url)
    {
        var request = new RestRequest($"/{subscriptionId}/{@event}", Method.Post)
            .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken)
            .AddBody(url);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Failed to subscribe to event {@event} for subscription {subscriptionId}");
        }
    }

    public async Task Unsubscribe(string @event, string subscriptionId, string url)
    {
        var requestGet = new RestRequest($"/{subscriptionId}/{@event}")
            .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        
        var webhooks = await _client.GetAsync<List<BridgeGetResponse>>(requestGet);
        var webhook = webhooks?.FirstOrDefault(w => w.Value == url);
        if (webhook != null)
        {
            var requestDelete = new RestRequest($"/{subscriptionId}/{@event}/{webhook.Id}", Method.Delete)
                .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            
            await _client.ExecuteAsync(requestDelete);
        }
    }
    
    public async Task<bool> IsAnySubscriberExist(string @event, string subscriptionId)
    {
        var request = new RestRequest($"/{subscriptionId}/{@event}")
            .AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        
        var response = await _client.GetAsync<List<BridgeGetResponse>>(request);
        return response?.Any() ?? false;
    }
}