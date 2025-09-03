using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
using Apps.AdobeWorkfront.Models.Requests;
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
    
    [TestMethod]
    public async Task CreateTask_WithValidData_ShouldCreateAndReturnTask()
    {
        var taskActions = new TaskActions(InvocationContext);
        var createRequest = new CreateTaskRequest
        {
            ProjectId = "68b16191000205545ecdee7125a2900c",
            Name = $"Test task from unit test {Guid.NewGuid()}",
            Priority = 2,
            AssigneeIds = new List<string> { "68a83efa006bc44521cf64d77b7989ba" }
        };

        var result = await taskActions.CreateTask(createRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(createRequest.Name, result.Name);
        Assert.AreEqual(createRequest.Priority, result.Priority);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task UpdateTask_WithValidData_ShouldUpdateAndReturnTask()
    {
        var taskActions = new TaskActions(InvocationContext);
        var updateRequest = new UpdateTaskRequest
        {
            TaskId = "68b7f2fc00540327c7e2da7baf4bb37f",
            Name = "Postman Task 6 [upd 2]",
            Status = "INP",
            Priority = 1,
            AssigneeIds = new List<string> { "68a83efa006bc44521cf64d77b7989ba" }
        };

        var result = await taskActions.UpdateTask(updateRequest);

        Assert.IsNotNull(result);
        Assert.AreEqual(updateRequest.Name, result.Name);
        Assert.AreEqual(updateRequest.Status, result.Status);
        Assert.AreEqual(updateRequest.Priority, result.Priority);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
}
