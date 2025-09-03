using Apps.AdobeWorkfront.Handlers;
using Apps.AdobeWorkfront.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.AdobeWorkfront.Models.Requests;

public class UploadFileRequest
{
    [Display("Parent type"), StaticDataSource(typeof(ParentTypeDataHandler))]
    public string ParentType { get; set; } = string.Empty;
    
    [Display("Parent ID"), DataSource(typeof(ParentDataHandler))]
    public string ParentId { get; set; } = string.Empty;
    
    [Display("File")]
    public FileReference File { get; set; } = default!;
}
