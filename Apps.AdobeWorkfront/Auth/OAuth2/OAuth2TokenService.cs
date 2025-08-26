using System.Globalization;
using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService
{
        public bool IsRefreshToken(Dictionary<string, string> values)
    {
        var expiresAt = DateTime.Parse(values[CredNames.ExpiresAt]);
        return DateTime.UtcNow > expiresAt;
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", values[CredNames.ClientId] },
            { "client_secret", values[CredNames.ClientSecret] },
            { "refresh_token", values[CredNames.RefreshToken] },
        };
        
        return GetToken(bodyParameters, cancellationToken);
    }

    public Task<Dictionary<string, string>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", values[CredNames.ClientId] },
            { "client_secret", values[CredNames.ClientSecret] },
            { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            { "code", code }
        };
        
        return GetToken(bodyParameters, cancellationToken);
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> GetToken(Dictionary<string, string> parameters,
        CancellationToken token)
    {
        var responseContent = await ExecuteTokenRequest(parameters, token);
        var tokenDto = JsonConvert.DeserializeObject<TokenDto>(responseContent!)!;

        var customExpiresIn = Math.Max(0, tokenDto.ExpiresIn - 10);
        var expiresAt = DateTime.UtcNow.AddSeconds(customExpiresIn);
        var expiresAtStr = expiresAt.ToString("o", CultureInfo.InvariantCulture);
        
        return new Dictionary<string, string>
        {
            { CredNames.AccessToken, tokenDto.AccessToken },
            { CredNames.RefreshToken, tokenDto.RefreshToken ?? string.Empty },
            { CredNames.ExpiresAt, expiresAtStr },
            { "token_type", tokenDto.TokenType ?? string.Empty }
        };
    }

    private async Task<string> ExecuteTokenRequest(Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        
        var tokenUrl = GetTokenUrl(parameters);
        using var response = await client.PostAsync(tokenUrl, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Error requesting token: {response.StatusCode} - {errorContent}");
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
    
    private string GetTokenUrl(Dictionary<string, string> values)
    {
        if(!values.TryGetValue(CredNames.BaseUrl, out var baseUrl))
        {
            throw new InvalidOperationException($"Base URL is not set. Values: {JsonConvert.SerializeObject(values)}");
        }
        
        return $"{baseUrl.TrimEnd('/')}/integrations/oauth2/api/v1/token";
    }
}