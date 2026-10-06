using Apps.AdobeWorkfront.Actions;
using Apps.AdobeWorkfront.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class CustomFieldActionsTests : TestBase
{
    [TestMethod]
    [DataRow("DE:test single select")]
    [DataRow("DE:test multi select")]
    [DataRow("DE:test checkboxes")]
    [DataRow("DE:test text")]
    [DataRow("DE:test rich text")]
    public async Task GetCustomFieldValue_WithValidTaskAndField_ShouldReturnFieldValue(string customFieldName)
    {
        // Arrange
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var request = new CustomFieldRequest
        {
            ParentType = "TASK",
            ParentId = "6ac3a65c0005a2b6bf939413f6ae5151",
            CustomField = customFieldName
        };

        // Act
        var result = await customFieldActions.GetCustomFieldValue(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.CustomFieldValue);
        PrintResult(result);
    }
    
    [TestMethod]
    [DataRow("DE:test number")]
    public async Task GetNumberCustomFieldValue_WithValidTaskAndField_ShouldReturnFieldValue(string customFieldName)
    {
        // Arrange
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var request = new CustomFieldRequest
        {
            ParentType = "TASK",
            ParentId = "6ac3a65c0005a2b6bf939413f6ae5151",
            CustomField = customFieldName
        };

        // Act
        var result = await customFieldActions.GetNumberCustomFieldValue(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.CustomFieldValue);
        PrintResult(result);
    }
    
    [TestMethod]
    [DataRow("DE:test date")]
    public async Task GetDateCustomFieldValue_WithValidTaskAndField_ShouldReturnFieldValue(string customFieldName)
    {
        // Arrange
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var request = new CustomFieldRequest
        {
            ParentType = "TASK",
            ParentId = "6ac3a65c0005a2b6bf939413f6ae5151",
            CustomField = customFieldName
        };

        // Act
        var result = await customFieldActions.GetDateCustomFieldValue(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.CustomFieldValue);
        PrintResult(result);
    }
    
    [TestMethod]
    [DataRow("DE:test multi select")]
    [DataRow("DE:test checkboxes")]
    public async Task GetMultipleValuesCustomFieldValue_WithValidTaskAndField_ShouldReturnFieldValue(string customFieldName)
    {
        // Arrange
        var customFieldActions = new CustomFieldActions(InvocationContext);
        var request = new CustomFieldRequest
        {
            ParentType = "TASK",
            ParentId = "6ac3a65c0005a2b6bf939413f6ae5151",
            CustomField = customFieldName
        };

        // Act
        var result = await customFieldActions.GetMultipleValuesCustomFieldValue(request);

        // Assert
        Assert.IsNotNull(result);
        PrintResult(result);
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
        PrintResult(result);
    }
}
