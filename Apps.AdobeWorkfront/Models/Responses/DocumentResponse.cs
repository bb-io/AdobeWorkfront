using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class DocumentResponse
{
    [Display("Document ID"), JsonProperty("ID")]
    public string DocumentId { get; set; } = string.Empty;
    
    [Display("Document name"), JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Download URL"), JsonProperty("downloadURL")]
    public string DownloadUrl { get; set; } = string.Empty;
}