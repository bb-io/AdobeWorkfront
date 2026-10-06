using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Project;

public class ProjectEntity
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}