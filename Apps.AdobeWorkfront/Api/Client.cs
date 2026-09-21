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
