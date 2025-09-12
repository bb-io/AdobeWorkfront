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
        var validProjectId = "68b54c4e01933961ed7e22eaca8a7b40"; 

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
    
    [TestMethod]
    public async Task CreateProject_WithValidData_ShouldCreateProject()
    {
        var projectActions = new ProjectActions(InvocationContext);
        var newProjectName = $"Test Project {Guid.NewGuid()}";

        var result = await projectActions.CreateProject(new()
        {
            Name = newProjectName,
            PlannedStartDate = DateTimeOffset.UtcNow.DateTime,
            Priority = 1,
            Status = "New"
        });

        Assert.IsNotNull(result);
        Assert.AreEqual(newProjectName, result.Name);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task UpdateProject_WithValidData_ShouldUpdateProject()
    {
        var projectActions = new ProjectActions(InvocationContext);
        var existingProjectId = "68b54ce70192648c87059e415737c6cf";
        var updatedProjectName = $"Updated Project {Guid.NewGuid()}";

        var result = await projectActions.UpdateProject(new()
        {
            ProjectId = existingProjectId,
            Name = updatedProjectName,
            PlannedStartDate = DateTimeOffset.UtcNow.AddDays(15).DateTime,
            Priority = 2
        });

        Assert.IsNotNull(result);
        Assert.AreEqual(updatedProjectName, result.Name);

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
    
    [TestMethod]
    public async Task DeleteProject_WithValidId_ShouldDeleteProject()
    {
        var projectActions = new ProjectActions(InvocationContext);
        var projectIdToDelete = "68b54faa01941eb54b5a7ab188282918";

        await projectActions.DeleteProject(new() { ProjectId = projectIdToDelete });
    }
}