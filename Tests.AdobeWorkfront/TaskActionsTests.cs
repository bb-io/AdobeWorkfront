using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class TaskActionsTests : TestBase
{
    [TestMethod]
    public async Task SearchTasks_WithoutFilters_ShouldReturnTasks()
    {
        var taskActions = new TaskActions(InvocationContext);

        var result = await taskActions.SearchTasks(new());

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Tasks);
        Assert.IsTrue(result.Tasks.Any());

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task GetTask_WithValidId_ShouldReturnTask()
    {
        var taskActions = new TaskActions(InvocationContext);
        var validTaskId = "68b5694d0001ee71e8aaa8f5980f43c1";

        var result = await taskActions.GetTask(new() { TaskId = validTaskId });

        Assert.IsNotNull(result);
        Assert.AreEqual(validTaskId, result.TaskId);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task GetTask_WithInvalidId_ShouldHandleError()
    {
        var taskActions = new TaskActions(InvocationContext);
        var invalidTaskId = "10000000000000000000000000000000";

        var exception = await Assert.ThrowsExceptionAsync<PluginApplicationException>(async () =>
        {
            await taskActions.GetTask(new() { TaskId = invalidTaskId });
        });
        
        Assert.IsNotNull(exception);
        Assert.AreEqual(exception.Message.Contains("not found", StringComparison.OrdinalIgnoreCase), true);
        
        Console.WriteLine($"Caught expected exception: {exception.Message}");
    }
}
