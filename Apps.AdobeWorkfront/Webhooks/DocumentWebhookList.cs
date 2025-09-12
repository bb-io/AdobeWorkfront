using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.DocumentHandler;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Documents")]
public class DocumentWebhookList(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On document uploaded", typeof(DocumentCreatedHandler), Description = "Triggers when a new document is uploaded")]
    public Task<WebhookResponse<DocumentResponse>> OnProjectCreated(WebhookRequest webhookRequest) => HandleWebhook<DocumentResponse>(webhookRequest, 
        payload => true);
}