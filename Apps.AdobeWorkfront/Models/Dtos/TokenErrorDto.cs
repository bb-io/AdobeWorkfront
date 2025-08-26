using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class TokenErrorDto
{
    [JsonProperty("statusCode")]
    public int StatusCode { get; set; }
        
    [JsonProperty("error")]
    public bool Error { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"({StatusCode}) {Type}: {Message}";
    }
}