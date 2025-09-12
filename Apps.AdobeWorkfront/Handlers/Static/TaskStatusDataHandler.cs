using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Handlers.Static;

public class TaskStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new("NEW", "New"),
            new("INP", "In Progress"),
            new("CPL", "Complete"),
            new("CPA", "Complete - Pending Approval"),
            new("CPI", "Complete - Pending Issues")
        ];
    }
}