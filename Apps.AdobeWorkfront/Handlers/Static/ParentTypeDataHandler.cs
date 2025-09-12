using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Handlers.Static;

public class ParentTypeDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new("PROJ", "Project"),
            new("TASK", "Task")
        ];
    }
}
