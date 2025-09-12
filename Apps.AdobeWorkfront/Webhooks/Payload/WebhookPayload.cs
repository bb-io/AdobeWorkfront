using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Webhooks.Payload;

public class WebhookPayload<T> where T : class
{
    [JsonProperty("newState")]
    public T NewState { get; set; } = null!;
    
    [JsonProperty("oldState")]
    public T OldState { get; set; } = null!;

    [JsonProperty("eventTime")]
    public EventTime EventTime { get; set; } = null!;
    
    [JsonProperty("subscriptionId")]
    public string SubscriptionId { get; set; } = null!;
    
    [JsonProperty("eventType")]
    public string EventType { get; set; } = null!;
    
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = null!;
    
    [JsonProperty("userId")]
    public string UserId { get; set; } = null!;
    
    [JsonProperty("subscriptionVersion")]
    public string SubscriptionVersion { get; set; } = null!;
    
    [JsonProperty("eventVersion")]
    public string EventVersion { get; set; } = null!;
}

public class EventTime
{
    [JsonProperty("epochSecond")]
    public long EpochSecond { get; set; }
    
    [JsonProperty("nano")]
    public int Nano { get; set; }
}