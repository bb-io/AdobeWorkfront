using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

// Has multiple schemas
// Example: {"title": "403 Forbidden", "description": "User is not an admin"}
public class ErrorWrapperDto
{
    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
    
    [JsonProperty("error")]
    public ErrorDto? Error { get; set; }
    
    public string? ExtractErrorMessage()
    {
        string? errorMessage = Error?.ToString();
        if (!string.IsNullOrWhiteSpace(errorMessage))
            return errorMessage;

        return !string.IsNullOrWhiteSpace(Title) 
            ? $"{Title.TrimEnd('.')}. {Description}".TrimEnd() 
            : null;
    }
}

public class ErrorDto
{
    [JsonProperty("class")]
    public string? Class { get; set; }
    
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    public override string ToString()
    {
        if(string.IsNullOrEmpty(Class))
        {
            return Message;
        }
        
        return $"{Class}: {Message}";
    }
}