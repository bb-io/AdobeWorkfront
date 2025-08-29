using Apps.AdobeWorkfront.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.AdobeWorkfront;

public class Invocable : BaseInvocable
{
    protected List<AuthenticationCredentialsProvider> Credentials =>
        InvocationContext.AuthenticationCredentialsProviders.ToList();

    protected Client Client { get; }

    protected Invocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new(Credentials);
    }
}
