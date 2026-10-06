using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Models.Requests.Comment;

public class AddCommentRequest
{
    [Display("Comment text")]
    public string CommentText { get; set; } = string.Empty;

    [Display("User IDs to tag"), DataSource(typeof(UserDataHandler))]
    public List<string>? TaggedUserIds { get; set; }
}