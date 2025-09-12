using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Custom fields")]
public class CustomFieldActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Get string custom field value", Description = "Get the value of a custom field for a specific object")]
    public async Task<StringResponse> GetCustomFieldValue([ActionParameter] CustomFieldRequest customFieldRequest)
    {
        var contentType = customFieldRequest.GetParentTypeForApi();
        var requestUrl = $"/attask/api/v19.0/{contentType}/{customFieldRequest.ParentId}";
        
        var apiRequest = new RestRequest(requestUrl)
            .AddQueryParameter("fields", $"parameterValues:*");
        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ObjectWithCustomFieldsDto>>(apiRequest);
        if (response.Data.CustomFields.TryGetValue(customFieldRequest.CustomField, out var customFieldValue))
        {
            if(customFieldValue is string strValue)
            {
                if(RichTextToPlainTextConverter.IsRichText(strValue))
                {
                    var plainText = RichTextToPlainTextConverter.ConvertToPlainText(strValue);
                    return new StringResponse(plainText);
                }
                
                return new StringResponse(strValue);
            }
        }
        
        return new StringResponse(string.Empty);
    }
    
    [Action("Set string custom field value", Description = "Set the value of a custom field for a specific object")]
    public async Task<StringResponse> SetCustomFieldValue([ActionParameter] SetCustomFieldValueRequest setCustomFieldValueRequest)
    {
        var contentType = setCustomFieldValueRequest.GetParentTypeForApi();
        var requestUrl = $"/attask/api/v19.0/{contentType}/{setCustomFieldValueRequest.ParentId}";
        
        var body = new
        {
            parameterValues = new Dictionary<string, string>
            {
                { setCustomFieldValueRequest.CustomField, setCustomFieldValueRequest.CustomFieldValue }
            }
        };
        
        var apiRequest = new RestRequest(requestUrl, Method.Put)
            .AddJsonBody(body);
        await Client.ExecuteWithErrorHandling<DataWrapperDto<ObjectWithCustomFieldsDto>>(apiRequest);
        return new StringResponse(setCustomFieldValueRequest.CustomFieldValue);
    }
}