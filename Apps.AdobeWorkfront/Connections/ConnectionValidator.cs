using Apps.AdobeWorkfront.Api;
using Apps.AdobeWorkfront.Models.Dtos;
using Apps.AdobeWorkfront.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.AdobeWorkfront.Connections;

public class FConnectionValidator: IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        var client = new Client(authenticationCredentialsProviders.ToList());
        var request = new RestRequest("/attask/api/v19.0/project/search");
        
        try
        {
            await client.ExecuteWithErrorHandling<DataWrapperDto<List<ProjectResponse>>>(request);
            return new()
            {
                IsValid = true
            };
        }
        catch (Exception e)
        {
            return new()
            {
                IsValid = false,
                Message = e.Message
            };
        }
    }
}