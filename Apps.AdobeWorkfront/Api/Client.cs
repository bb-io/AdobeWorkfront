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
        BaseUrl = new Uri(credentialsProviders.GetBaseUrl()),
    })
    {
        this.AddDefaultHeader(credentialsProviders.GetTokenType(), credentialsProviders.GetAccessToken());
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                return new PluginApplicationException($"Got an error with status code: {response.StatusCode}");
            }

            return new PluginApplicationException(response.ErrorMessage);
        }
        
        if(response.ContentType == "text/html")
        {
            return new PluginApplicationException($"Got an error with status code: {response.StatusCode} and content: {response.Content}");
        }
        
        var errorResponse = JsonConvert.DeserializeObject<ErrorWrapperDto>(response.Content!)!;
        return new PluginApplicationException(errorResponse.ToString());
    }
}
