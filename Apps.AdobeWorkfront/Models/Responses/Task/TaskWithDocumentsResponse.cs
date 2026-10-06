using Apps.AdobeWorkfront.Models.Dtos.Document;
using Apps.AdobeWorkfront.Models.Entities.Task;
using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses.Task;

public class TaskWithDocumentsResponse : TaskResponse
{
    public TaskWithDocumentsResponse(TaskWithDocumentsEntity entity) : base(entity)
    {
        Documents = entity.Documents?.Select(x => new DocumentDto(x));
    }
    
    [Display("Documents")]
    public IEnumerable<DocumentDto>? Documents { get; set; }
}