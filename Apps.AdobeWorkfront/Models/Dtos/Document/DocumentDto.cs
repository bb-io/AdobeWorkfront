using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Dtos.Document;

public record DocumentDto
{
    public DocumentDto(DocumentResponse response)
    {
        DocumentId = response.DocumentId;
        Name = response.Name;
        DownloadUrl = response.DownloadUrl;
        FileExtension = response.CurrentVersion.Ext;
    }
    
    [Display("Document ID")]
    public string DocumentId { get; set; } = string.Empty;
    
    [Display("Document name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Download URL")]
    public string DownloadUrl { get; set; } = string.Empty;
    
    [Display("File extension")]
    public string FileExtension { get; set; }
}