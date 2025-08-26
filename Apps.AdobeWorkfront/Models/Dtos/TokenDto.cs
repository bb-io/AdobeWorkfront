using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class TokenDto
{
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;
        
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonProperty("wid")]
    public string Wid { get; set; } = string.Empty;
}