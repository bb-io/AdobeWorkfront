using Apps.AdobeWorkfront.Models.Entities.Document;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Dtos.Document;

public record DocumentDto
{
    public DocumentDto(DocumentResponse response)
        : this(response.DocumentId, response.Name, response.DownloadUrl, response.CurrentVersion?.Ext) { }

    public DocumentDto(DocumentEntity entity)
        : this(entity.DocumentId, entity.Name, entity.DownloadUrl, entity.CurrentVersion?.Ext) { }

    private DocumentDto(string documentId, string name, string downloadUrl, string? fileExtension)
    {
        DocumentId = documentId;
        Name = name;
        DownloadUrl = downloadUrl;
        FileExtension = fileExtension ?? string.Empty;
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