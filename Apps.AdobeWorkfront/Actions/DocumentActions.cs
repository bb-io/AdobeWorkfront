using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Documents")]
public class DocumentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : Invocable(invocationContext)
{
    private const string DocumentFields = "downloadURL";
    
    [Action("Upload file", Description = "Upload a file and attach it to a task or project")]
    public async Task UploadFile([ActionParameter] UploadFileRequest request)
    {
        var handle = await UploadFile(request.File);
        await CreateDocumentFromHandle(handle, request.File.Name, request.ParentType, request.ParentId);
    }
    
    [Action("Download document", Description = "Download a document by its ID")]
    public async Task<FileResponse> DownloadFile([ActionParameter] DocumentRequest request)
    {
        var getDocRequest = new RestRequest($"/attask/api/v19.0/document/{request.DocumentId}")
            .AddQueryParameter("fields", DocumentFields);
        var docResponse = await Client.ExecuteWithErrorHandling<DataWrapperDto<DocumentResponse>>(getDocRequest);
        if (string.IsNullOrEmpty(docResponse.Data.DownloadUrl))
        {
            throw new PluginApplicationException("Document does not have a download URL.");
        }
        
        var downloadDocumentRequest = new RestRequest(docResponse.Data.DownloadUrl);
        var downloadResponse = await Client.ExecuteWithErrorHandling(downloadDocumentRequest);
        var memoryStream = new MemoryStream(downloadResponse.RawBytes!);
        memoryStream.Position = 0;
        
        var fileName = downloadResponse.ContentHeaders!.GetFileName();
        var contentType = MimeTypes.GetMimeType(fileName);
        var fileReference = await fileManagementClient.UploadAsync(memoryStream, contentType, fileName);
        return new(fileReference);
    }

    private async Task<string> UploadFile(FileReference file)
    {
        var fileStream = await fileManagementClient.DownloadAsync(file);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var bytes = memoryStream.ToArray();
        var uploadRequest = new RestRequest("/attask/api/v19.0/upload", Method.Post)
        {
            AlwaysMultipartFormData = true
        };

        uploadRequest.AddFile("uploadedFile", bytes, file.Name);
        
        var uploadResponse = await Client.ExecuteWithErrorHandling<DataWrapperDto<UploadResponse>>(uploadRequest);
        return uploadResponse.Data.Handle;
    }

    private async Task CreateDocumentFromHandle(string handle, string fileName, string parentType, string parentId)
    {
        var createDocRequest = new RestRequest("/attask/api/v19.0/document", Method.Post)
            .AddParameter("name", fileName, ParameterType.GetOrPost)
            .AddParameter("handle", handle, ParameterType.GetOrPost)
            .AddParameter("docObjCode", parentType, ParameterType.GetOrPost)
            .AddParameter("objID", parentId, ParameterType.GetOrPost);
        
        await Client.ExecuteWithErrorHandling(createDocRequest);
    }
}