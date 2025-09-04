using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.AdobeWorkfront.Models.Requests;

public class UploadFileRequest : ParentRequest
{
    [Display("File")]
    public FileReference File { get; set; } = default!;
}
