using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.AdobeWorkfront.Api;

public class Client : BlackBirdRestClient
{
    public Client(List<AuthenticationCredentialsProvider> credentialsProviders) : base(new()
    {
        BaseUrl = new Uri(credentialsProviders.GetBaseUrl())
    })
    {
        this.AddDefaultHeader(credentialsProviders.GetTokenType(), credentialsProviders.GetAccessToken());
    }

    public async Task<List<T>> Paginate<T>(RestRequest request)
    {
        const int limit = 100;
        int offset = 0;

        // https://experienceleague.adobe.com/en/docs/workfront/using/adobe-workfront-api/api-general-information/api-basics#using-paginated-responses
        // "To make sure your results are properly paginated, use a sorting parameter.
        // This allows the results to be returned in the same order, so that the pagination does not repeat or skip results"
        if (request.Parameters.All(p => p.Name?.EndsWith("_Sort", StringComparison.Ordinal) != true))
            request.AddQueryParameter("ID_Sort", "asc");

        var results = new List<T>();

        while (true)
        {
            request.AddOrUpdateParameter("$$LIMIT", limit);
            request.AddOrUpdateParameter("$$FIRST", offset);

            var response = await ExecuteWithErrorHandling<DataWrapperDto<List<T>>>(request);
            results.AddRange(response.Data);

            if (response.Data.Count < limit)
                return results;

            offset += limit;
        }
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        string statusCodePart = $"Got an error with status code: {response.StatusCode}";
        
        if (string.IsNullOrEmpty(response.Content))
        {
            return string.IsNullOrEmpty(response.ErrorMessage)
                ? new PluginApplicationException(statusCodePart) 
                : new PluginApplicationException(response.ErrorMessage);
        }
        
        string rawResponse = response.Content[..Math.Min(response.Content.Length, 300)];
        
        try
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorWrapperDto>(response.Content);
            string? errorMessage = errorResponse?.ExtractErrorMessage();

            if (errorResponse is null || string.IsNullOrWhiteSpace(errorMessage))
                return new PluginApplicationException($"{statusCodePart} - couldn't deserialize a JSON error. Raw: {rawResponse}");

            return new PluginApplicationException(errorMessage);
        }
        catch (JsonException)
        {
            return new PluginApplicationException($"{statusCodePart} and content: {rawResponse}");
        }
    }
}
