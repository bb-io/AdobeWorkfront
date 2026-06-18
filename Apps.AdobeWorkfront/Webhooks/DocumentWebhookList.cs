using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Webhooks.Handlers.DocumentHandler;
using Apps.AdobeWorkfront.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.AdobeWorkfront.Webhooks;

[WebhookList("Documents")]
public class DocumentWebhookList(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On document uploaded", typeof(DocumentCreatedHandler),
        Description = "Triggers when a new document is uploaded")]
    public async Task<WebhookResponse<OnDocumentUploadedResponse>> OnProjectCreated(WebhookRequest webhookRequest)
    {
        var webhookResponse = await HandleWebhook<DocumentResponse>(webhookRequest, payload => true);
        return new WebhookResponse<OnDocumentUploadedResponse>
        {
            ReceivedWebhookRequestType = webhookResponse.ReceivedWebhookRequestType, 
            HttpResponseMessage = webhookResponse.HttpResponseMessage,
            Result = webhookResponse.Result is null ? null : new OnDocumentUploadedResponse(webhookResponse.Result) 
        };
    }
}