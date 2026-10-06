using Apps.AdobeWorkfront.Models.Entities.Document;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Entities.Task;

public class TaskWithDocumentsEntity : TaskFullEntity
{
    [JsonProperty("documents")]
    public IEnumerable<DocumentEntity>? Documents { get; set; }
}