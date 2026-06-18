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

    [TestMethod]
    public async Task SearchTaskDocuments_WithValidTaskId_ShouldReturnDocuments()
    {
        // Arrange
        var actions = new DocumentActions(InvocationContext, FileManager);
        var taskInput = new TaskRequest { TaskId = "68b7f0ea001df04b8a02dc190fa1b1a0" };

        // Act
        var result = await actions.SearchTaskDocuments(taskInput);

        // Assert
        Assert.IsNotNull(result);
        PrintResult(result);
    }
    
    [TestMethod]
    public async Task SearchProjectDocuments_WithValidTaskId_ShouldReturnDocuments()
    {
        // Arrange
        var actions = new DocumentActions(InvocationContext, FileManager);
        var projectInput = new ProjectRequest { ProjectId = "68b16191000205545ecdee7125a2900c" };

        // Act
        var result = await actions.SearchProjectDocuments(projectInput);

        // Assert
        Assert.IsNotNull(result);
        PrintResult(result);
    }
}
