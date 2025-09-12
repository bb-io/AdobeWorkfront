using Apps.AdobeWorkfront.Handlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class CustomFieldDataHandlerTests : BaseDataHandlerTests
{
    protected override IAsyncDataSourceItemHandler DataHandler => new CustomFieldDataHandler(InvocationContext, new()
    {
        ParentType = "TASK",
        ParentId = "68b943890000b3f9a2461de5fe76b61b"
    });

    protected override string SearchString => "custom";
}