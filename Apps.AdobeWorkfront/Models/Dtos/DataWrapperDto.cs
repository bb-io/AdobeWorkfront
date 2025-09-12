using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class DataWrapperDto<T>
{
    [JsonProperty("data")]
    public T Data { get; set; } = default!;
}