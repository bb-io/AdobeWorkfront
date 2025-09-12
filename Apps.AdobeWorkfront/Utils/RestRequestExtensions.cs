using RestSharp;
using QueryParameter = Apps.AdobeWorkfront.Models.Entities.QueryParameter;

namespace Apps.AdobeWorkfront.Utils;

public static class RestRequestExtensions
{
    public static void ApplyToRequest(this RestRequest request, List<QueryParameter> queryParameters)
    {
        foreach (var queryParameter in queryParameters)
        {
            request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
        }
    }
}