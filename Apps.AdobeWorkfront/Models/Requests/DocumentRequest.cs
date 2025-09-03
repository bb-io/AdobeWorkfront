using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class DocumentRequest
{
    [Display("Document ID"), DataSource(typeof(DocumentDataHandler))]
    public string DocumentId { get; set; } = string.Empty;
}