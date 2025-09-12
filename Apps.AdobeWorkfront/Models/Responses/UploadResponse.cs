using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class UploadResponse
{
    [JsonProperty("handle")]
    public string Handle { get; set; } = string.Empty;
}