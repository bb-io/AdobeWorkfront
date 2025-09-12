using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class FilterDto
{
    [JsonProperty("fieldName")]
    public string FieldName { get; set; } = string.Empty;

    [JsonProperty("fieldValue")]
    public string FieldValue { get; set; } = string.Empty;
    
    [JsonProperty("comparison")]
    public string Comparison { get; set; } = string.Empty;
}