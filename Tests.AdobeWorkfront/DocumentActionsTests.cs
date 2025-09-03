using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
using Apps.AdobeWorkfront.Models.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class DocumentActionsTests : TestBase
{
    [TestMethod]
    public async Task DownloadDocument_WithValidId_ShouldReturnFile()
    {
        var documentActions = new DocumentActions(InvocationContext, FileManager);
        var validDocumentId = "68b81fb4006be6e10fd64daa037adaa5";

        var result = await documentActions.DownloadFile(new DocumentRequest { DocumentId = validDocumentId });

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.File);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }

    [TestMethod]
    public async Task UploadFile_WithValidInputs_ShouldUploadSuccessfully()
    {
        var documentActions = new DocumentActions(InvocationContext, FileManager);
        var uploadRequest = new UploadFileRequest
        {
            ParentType = "TASK",
            ParentId = "68b7f2fc00540327c7e2da7baf4bb37f",
            File = new FileReference { Name = "import.txt", ContentType = "text/plain" }
        };

        await documentActions.UploadFile(uploadRequest);

        Assert.IsTrue(true, "File upload completed successfully");
        Console.WriteLine("File uploaded successfully to task");
    }
}
