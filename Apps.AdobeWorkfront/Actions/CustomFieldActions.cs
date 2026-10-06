using System.Globalization;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Requests;
using Apps.AdobeWorkfront.Models.Responses.CustomField;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.AdobeWorkfront.Actions;

[ActionList("Custom fields")]
public class CustomFieldActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Get string custom field value", 
        Description = "Get the value of a custom field for a specific object. Multi-value fields are returned comma-separated")]
    public async Task<StringResponse> GetCustomFieldValue([ActionParameter] CustomFieldRequest customFieldRequest)
    {
        var raw = await GetRawValue(customFieldRequest);
        return new StringResponse(string.Join(", ", CustomFieldValueParser.ToValues(raw)));
    }
    
    [Action("Get number custom field value", Description = "Get the value of a number custom field")]
    public async Task<NumberResponse> GetNumberCustomFieldValue([ActionParameter] CustomFieldRequest request)
    {
        var raw = await GetRawValue(request);
        if (raw is null)
            return new(null);

        string? converted = Convert.ToString(raw, CultureInfo.InvariantCulture);
        return decimal.TryParse(converted, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)
            ? new(number)
            : throw new PluginMisconfigurationException($"Custom field '{request.CustomField}' is not a number: '{converted}'");
    }

    [Action("Get date custom field value", Description = "Get the value of a date custom field")]
    public async Task<DateResponse> GetDateCustomFieldValue([ActionParameter] CustomFieldRequest request)
    {
        var raw = await GetRawValue(request);
        if (raw is null)
            return new(null);

        var str = raw.ToString()!;
        return WorkfrontDateTime.TryParse(str, out var date)
            ? new(date)
            : throw new PluginMisconfigurationException($"Custom field '{request.CustomField}' is not a date: '{str}'");
    }

    [Action("Get multiple values custom field value", Description = "Get the values of a multi-select or checkbox custom field")]
    public async Task<MultipleValuesResponse> GetMultipleValuesCustomFieldValue([ActionParameter] CustomFieldRequest request)
    {
        var raw = await GetRawValue(request);
        return new(CustomFieldValueParser.ToValues(raw));
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
    
    private async Task<object?> GetRawValue(CustomFieldRequest request)
    {
        var apiRequest = new RestRequest($"/attask/api/v19.0/{request.GetParentTypeForApi()}/{request.ParentId}")
            .AddQueryParameter("fields", "parameterValues:*");

        var response = await Client.ExecuteWithErrorHandling<DataWrapperDto<ObjectWithCustomFieldsDto>>(apiRequest);
        return response.Data.CustomFields.GetValueOrDefault(request.CustomField);
    }
}