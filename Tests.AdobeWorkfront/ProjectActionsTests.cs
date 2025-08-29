using System.Text.Json;
using Apps.AdobeWorkfront.Actions;
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

        var result = await projectActions.SearchProjects();

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Projects);
        Assert.IsTrue(result.Projects.Any());

        Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
}