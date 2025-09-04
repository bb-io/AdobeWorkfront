using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class ObjectWithCustomFieldsDto
{
    [JsonProperty("ID")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("parameterValues")]
    public Dictionary<string, object> CustomFields { get; set; } = new();
}