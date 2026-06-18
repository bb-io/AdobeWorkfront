using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class GetTaskResponse : TaskResponse
{
    [Display("Documents"), JsonProperty("documents")]
    public IEnumerable<DocumentResponse>? Documents { get; set; }
}