using Apps.AdobeWorkfront.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Auth.OAuth2;

public class OAuth2AuthorizeService(InvocationContext invocationContext) : BaseInvocable(invocationContext), IOAuth2AuthorizeService
{
    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var bridgeOauthUrl = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/oauth";
        var oauthUrl = GetOAuthUrl(values);
        var parameters = new Dictionary<string, string>
        {
            { CredNames.ClientId, values[CredNames.ClientId] },
            { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            { "actual_redirect_uri", InvocationContext.UriInfo.ImplicitGrantRedirectUri.ToString() },
            { "authorization_url", oauthUrl},            
            { "response_type", "code" },
            { "state", values["state"] }
        };
        
        return QueryHelpers.AddQueryString(bridgeOauthUrl, parameters!);
    }

    private string GetOAuthUrl(Dictionary<string, string> values)
    {
        if(!values.TryGetValue(CredNames.BaseUrl, out var baseUrl))
        {
            throw new InvalidOperationException($"Base URL is not set. Values: {JsonConvert.SerializeObject(values)}");
        }
        
        return $"{baseUrl.TrimEnd('/')}/integrations/oauth2/authorize";
    }
}