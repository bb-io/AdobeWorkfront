using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Document;

public class DocumentEntity
{
    [JsonProperty("ID")]
    public string DocumentId { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("downloadURL")]
    public string DownloadUrl { get; set; } = string.Empty;

    [JsonProperty("currentVersion")] 
    public DocumentVersion CurrentVersion { get; set; } = null!;
}