using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Requests.Comment;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Comments")]
public class CommentActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Create comment", Description = "Creates a new comment to an existing task or project")]
    public async Task CreateComment(
        [ActionParameter] ParentRequest parentRequest,
        [ActionParameter] AddCommentRequest input)
    {
        var body = new
        {
            noteObjCode = parentRequest.ParentType,
            objID = parentRequest.ParentId,
            noteText = input.CommentText,
            tags = (input.TaggedUserIds ?? []).Select(id => new { objID = id, objObjCode = "USER" })
        };

        var apiRequest = new RestRequest("/attask/api/v19.0/note", Method.Post).AddJsonBody(body);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }
}