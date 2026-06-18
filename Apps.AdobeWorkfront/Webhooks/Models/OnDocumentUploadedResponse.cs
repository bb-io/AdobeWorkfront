using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Webhooks.Models;

public record OnDocumentUploadedResponse
{
    public OnDocumentUploadedResponse(DocumentResponse document)
    {
        DocumentId = document.DocumentId;
        Name = document.Name;
        DownloadUrl = document.DownloadUrl;
    }

    [Display("Document ID")]
    public string DocumentId { get; set; }
    
    [Display("Document name")]
    public string Name { get; set; }
    
    [Display("Download URL")]
    public string DownloadUrl { get; set; }
}