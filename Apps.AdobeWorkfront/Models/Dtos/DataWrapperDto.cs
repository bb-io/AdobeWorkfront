using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class DataWrapperDto<T>
{
    [JsonProperty("data")]
    public List<T> Data { get; set; } = new();
}