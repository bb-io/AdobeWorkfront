using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.AdobeWorkfront.Base;

namespace Tests.AdobeWorkfront;

[TestClass]
public class ProjectActionsTests : TestBase
{
    [TestMethod]
    public async Task SearchProjects_WithoutFilters_ShouldReturnProjects()
    {
        var projectActions = new ProjectActions(InvocationContext);

        var result = await projectActions.SearchProjects(new());

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Projects);
        Assert.IsTrue(result.Projects.Any());

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task SearchProjects_WithDateFilter_ShouldReturnProjects()
    {
        var projectActions = new ProjectActions(InvocationContext);

        var result = await projectActions.SearchProjects(new()
        {
            PlannedStartDateFrom = DateTimeOffset.Parse("2025-08-28T11:00:00.000-06:00").DateTime
        });

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Projects);
        Assert.IsTrue(result.Projects.Any());

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task GetProject_WithValidId_ShouldReturnProject()
    {
        var projectActions = new ProjectActions(InvocationContext);
        var validProjectId = "68b16191000205545ecdee7125a2900c"; 

        var result = await projectActions.GetProject(new() { ProjectId = validProjectId });

        Assert.IsNotNull(result);
        Assert.AreEqual(validProjectId, result.ProjectId);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task GetProject_WithInvalidId_ShouldHandleError()
    {
        var projectActions = new ProjectActions(InvocationContext);
        var invalidProjectId = "10000000000000000000000000000000";

        var exception = await Assert.ThrowsExceptionAsync<PluginApplicationException>(async () =>
        {
            await projectActions.GetProject(new() { ProjectId = invalidProjectId });
        });
        
        Assert.IsNotNull(exception);
        Assert.AreEqual(exception.Message.Contains("not found", StringComparison.OrdinalIgnoreCase), true);
        
        Console.WriteLine($"Caught expected exception: {exception.Message}");
    }
}