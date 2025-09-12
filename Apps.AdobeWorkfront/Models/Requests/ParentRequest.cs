using Apps.AdobeWorkfront.Handlers;
using Apps.AdobeWorkfront.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class ParentRequest
{
    [Display("Parent type"), StaticDataSource(typeof(ParentTypeDataHandler))]
    public string ParentType { get; set; } = string.Empty;
    
    [Display("Parent ID"), DataSource(typeof(ParentDataHandler))]
    public string ParentId { get; set; } = string.Empty;
    
    public string GetParentTypeForApi()
    {
        return ParentType switch
        {
            "PROJ" => "project",
            "TASK" => "task",
            _ => throw new ArgumentOutOfRangeException(nameof(ParentType), $"Unsupported parent type: {ParentType}")
        };
    }
}