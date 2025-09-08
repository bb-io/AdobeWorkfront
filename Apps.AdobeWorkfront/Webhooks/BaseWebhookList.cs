using System.Net;
using Apps.AdobeWorkfront.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Webhooks;

public abstract class BaseWebhookList(InvocationContext invocationContext) : Invocable(invocationContext)
{
    protected virtual Task<WebhookResponse<T>> HandleWebhook<T>(WebhookRequest webhookRequest, Func<WebhookPayload<T>, bool> triggerFlight) where T : class
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

        var authHeader = webhookRequest.Headers.FirstOrDefault(h => h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase));
        var responseMessage = new HttpResponseMessage(HttpStatusCode.Accepted);
        if (!string.IsNullOrEmpty(authHeader.Value))
        {
            responseMessage.Headers.Add("Authorization", authHeader.Value);
        }
        
        if (triggerFlight.Invoke(payload) == false)
        {
            return Task.FromResult(new WebhookResponse<T>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = payload.NewState,
                HttpResponseMessage = responseMessage
            });
        }

        return Task.FromResult(new WebhookResponse<T>
        {
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = payload.NewState,
            HttpResponseMessage = responseMessage
        });
    }
}