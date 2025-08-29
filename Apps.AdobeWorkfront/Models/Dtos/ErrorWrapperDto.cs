using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class ErrorWrapperDto
{
    [JsonProperty("error")]
    public ErrorDto Error { get; set; } = new();
    
    public override string ToString()
    {
        return Error.ToString();
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