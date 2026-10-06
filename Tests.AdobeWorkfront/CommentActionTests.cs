using Apps.AdobeWorkfront.Actions;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Requests.Comment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class CommentActionTests : TestBase
{
    [TestMethod]
    public async Task CreateComment_IsSuccess()
    {
        // Arrange
        var actions = new CommentActions(InvocationContext);
        var parentRequest = new ParentRequest
        {
            ParentType = "PROJ",
            ParentId = "68b16191000205545ecdee7125a2900c"
        };
        var input = new AddCommentRequest
        {
            CommentText = "test from tests1",
            TaggedUserIds = ["68a83efa006bc44521cf64d77b7989ba"]
        };

        // Act
        await actions.CreateComment(parentRequest, input);
    }
}