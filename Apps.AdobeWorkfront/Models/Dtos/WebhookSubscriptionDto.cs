using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Dtos;

public class WebhookSubscriptionDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;
    
    [JsonProperty("dateCreated")]
    public string DateCreated { get; set; } = default!;
    
    [JsonProperty("dateModified")]
    public string DateModified { get; set; } = default!;
    
    [JsonProperty("authToken")]
    public string AuthToken { get; set; } = default!;
    
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = default!;
    
    [JsonProperty("eventType")]
    public string EventType { get; set; } = default!;
    
    [JsonProperty("objCode")]
    public string ObjCode { get; set; } = default!;
    
    [JsonProperty("objId")]
    public string ObjId { get; set; } = default!;
    
    [JsonProperty("url")]
    public string Url { get; set; } = default!;
    
    [JsonProperty("base64Encoding")]
    public bool Base64Encoding { get; set; }
    
    [JsonProperty("filters")]
    public List<FilterDto> Filters { get; set; } = new();
    
    [JsonProperty("filterConnector")]
    public string FilterConnector { get; set; } = default!;
    
    [JsonProperty("version")]
    public string Version { get; set; } = default!;
    
    [JsonProperty("dateVersionUpdated")]
    public string DateVersionUpdated { get; set; } = default!;
}

public class WebhookSubscriptionsResponseDto
{
    [JsonProperty("subscriptions")]
    public List<WebhookSubscriptionDto> Subscriptions { get; set; } = new();
    
    [JsonProperty("meta")]
    public MetaDto Meta { get; set; } = default!;
}

public class CreateWebhookRequestDto
{
    [JsonProperty("objCode")]
    public string ObjCode { get; set; } = default!;
    
    [JsonProperty("eventType")]
    public string EventType { get; set; } = default!;
    
    [JsonProperty("url")]
    public string Url { get; set; } = default!;
    
    [JsonProperty("authToken")]
    public string AuthToken { get; set; } = default!;
    
    [JsonProperty("filters")]
    public List<FilterDto> Filters { get; set; } = new();
}

public class CreateWebhookResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;
    
    [JsonProperty("version")]
    public string Version { get; set; } = default!;
}

public class MetaDto
{
    [JsonProperty("page")]
    public int Page { get; set; }
    
    [JsonProperty("limit")]
    public int Limit { get; set; }
    
    [JsonProperty("page_count")]
    public int PageCount { get; set; }
    
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }
}
