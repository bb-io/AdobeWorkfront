using Apps.AdobeWorkfront.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.AdobeWorkfront.Utils;

public static class AuthenticationCredentialsProviderExtensions
{
    public static string GetBaseUrl(this IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var baseUrlProvider = credentialsProviders.Get(CredNames.BaseUrl);
        if(string.IsNullOrEmpty(baseUrlProvider?.Value))
        {
            throw new Exception("Base URL is not provided in the authentication credentials.");
        }
        
        return baseUrlProvider.Value.TrimEnd('/');
    }
    
    public static string GetTokenType(this IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var tokenTypeProvider = credentialsProviders.Get(CredNames.TokenType);
        if(string.IsNullOrEmpty(tokenTypeProvider?.Value))
        {
            throw new Exception("Token type is not provided in the authentication credentials.");
        }
        
        return tokenTypeProvider.Value;
    }
    
    public static string GetAccessToken(this IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var accessTokenProvider = credentialsProviders.Get(CredNames.AccessToken);
        if(string.IsNullOrEmpty(accessTokenProvider?.Value))
        {
            throw new Exception("Access token is not provided in the authentication credentials.");
        }
        
        return accessTokenProvider.Value;
    }
}