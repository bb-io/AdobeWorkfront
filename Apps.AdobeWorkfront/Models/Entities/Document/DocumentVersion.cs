using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Document;

public class DocumentVersion
{
    [JsonProperty("ext")]
    public string Ext { get; set; } = string.Empty;
}