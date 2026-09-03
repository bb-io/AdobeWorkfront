using System.Globalization;
using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService, ITokenRefreshable
{
    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        var expiresAt = DateTime.Parse(values[CredNames.ExpiresAt]);
        return DateTime.UtcNow > expiresAt;
    }

    public int? GetRefreshTokenExprireInMinutes(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(CredNames.ExpiresAt, out var expireValue))
            return null;

        if (!DateTime.TryParse(expireValue, out var expireDate))
            return null;

        double minutes = (expireDate - DateTime.UtcNow).TotalMinutes - 5;
        return Math.Max(1, (int)minutes);
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        string refreshToken = values.GetValueOrDefault(CredNames.RefreshToken, string.Empty);
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            string dictString = string.Join(Environment.NewLine, values.Where(x => !x.Key.Equals(CredNames.ClientSecret)));
            InvocationContext.Logger?.LogError($"No refresh token found by this key: {CredNames.RefreshToken}. Values: {dictString}", []);
            throw new PluginApplicationException("No refresh token is stored. Please reconnect");
        }
        
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", values[CredNames.ClientId] },
            { "client_secret", values[CredNames.ClientSecret] },
            { "refresh_token", refreshToken },
        };
        
        return GetToken(bodyParameters, values, cancellationToken);
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
        
        return GetToken(bodyParameters, values, cancellationToken);
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> GetToken(Dictionary<string, string> bodyParameters,
        Dictionary<string, string> values,
        CancellationToken token)
    {
        var responseContent = await ExecuteTokenRequest(bodyParameters, values, token);
        var tokenDto = JsonConvert.DeserializeObject<TokenDto>(responseContent);

        if (tokenDto is null || string.IsNullOrWhiteSpace(tokenDto.AccessToken) || string.IsNullOrWhiteSpace(tokenDto.RefreshToken))
        {
            InvocationContext.Logger?.LogError($"Unexpected TokenDto body shape. Raw: {responseContent}", []);
            throw new PluginApplicationException("Unexpected authentication response received from Workfront. Please reconnect");
        }
        
        var expiresAt = DateTime.UtcNow.AddSeconds(tokenDto.ExpiresIn);
        var expiresAtStr = expiresAt.ToString("o", CultureInfo.InvariantCulture);
        
        return new Dictionary<string, string>
        {
            { CredNames.AccessToken, tokenDto.AccessToken },
            { CredNames.RefreshToken, tokenDto.RefreshToken },
            { CredNames.ExpiresAt, expiresAtStr },
            { "token_type", tokenDto.TokenType }
        };
    }

    private async Task<string> ExecuteTokenRequest(Dictionary<string, string> bodyParameters,
        Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(bodyParameters);
        
        var tokenUrl = GetTokenUrl(values);
        using var response = await client.PostAsync(tokenUrl, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var tokenErrorDto = JsonConvert.DeserializeObject<TokenErrorDto>(errorContent)!;
            throw new Exception(tokenErrorDto.ToString());
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