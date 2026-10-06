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
        ParentId = "6ac3a65c0005a2b6bf939413f6ae5151"
    });

    protected override string SearchString => "custom";
}