using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Documents")]
public class DocumentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : Invocable(invocationContext)
{
    [Action("Upload file", Description = "Upload a file and attach it to a task or project")]
    public async Task UploadFile([ActionParameter] UploadFileRequest request)
    {
        var handle = await UploadFile(request.File);
        await CreateDocumentFromHandle(handle, request.File.Name, request.ParentType, request.ParentId);
    }

    private async Task<string> UploadFile(FileReference file)
    {
        var fileStream = await fileManagementClient.DownloadAsync(file);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var uploadRequest = new RestRequest("/attask/api/v19.0/upload", Method.Post);
        uploadRequest.AddFile("uploadedFile", () => memoryStream, file.Name);
        
        var uploadResponse = await Client.ExecuteWithErrorHandling<DataWrapperDto<UploadResponse>>(uploadRequest);
        return uploadResponse.Data.Handle;
    }

    private async Task CreateDocumentFromHandle(string handle, string fileName, string parentType, string parentId)
    {
        var createDocRequest = new RestRequest("/attask/api/v19.0/document", Method.Post)
            .AddParameter("name", Path.GetFileNameWithoutExtension(fileName), ParameterType.GetOrPost)
            .AddParameter("handle", handle, ParameterType.GetOrPost)
            .AddParameter("docObjCode", parentType, ParameterType.GetOrPost)
            .AddParameter("objID", parentId, ParameterType.GetOrPost);
        
        await Client.ExecuteWithErrorHandling(createDocRequest);
    }
}