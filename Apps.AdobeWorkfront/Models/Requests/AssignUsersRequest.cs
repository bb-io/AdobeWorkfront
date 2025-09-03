using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests;

public class AssignUsersRequest : TaskRequest
{
    [Display("User IDs"), DataSource(typeof(UserDataHandler))]
    public IEnumerable<string> UserIds { get; set; } = [];
}