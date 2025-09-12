using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
using Apps.AdobeWorkfront.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class CustomFieldActionsTests : TestBase
{
    [TestMethod]
    public async Task GetCustomFieldValue_WithValidTaskAndField_ShouldReturnFieldValue()
    {
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var request = new CustomFieldRequest
        {
            ParentType = "TASK",
            ParentId = "68b943890000b3f9a2461de5fe76b61b",
            CustomField = "DE:Executive Interest"
        };

        var result = await customFieldActions.GetCustomFieldValue(request);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.CustomFieldValue);
        Assert.AreEqual("Yes", result.CustomFieldValue);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }

    [TestMethod]
    public async Task SetCustomFieldValue_WithValidTaskAndField_ShouldSetAndReturnFieldValue()
    {
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var guid = Guid.NewGuid().ToString();
        var request = new SetCustomFieldValueRequest
        {
            ParentType = "TASK",
            ParentId = "68b943890000b3f9a2461de5fe76b61b",
            CustomField = "DE:Vitalii custom field",
            CustomFieldValue = $"Updated value {guid}"
        };

        var result = await customFieldActions.SetCustomFieldValue(request);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.CustomFieldValue);
        Assert.AreEqual($"Updated value {guid}", result.CustomFieldValue);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
}
