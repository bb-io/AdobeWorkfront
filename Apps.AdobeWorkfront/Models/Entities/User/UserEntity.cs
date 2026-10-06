using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.User;

public class UserEntity
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("emailAddr")]
    public string? Email { get; set; }
}