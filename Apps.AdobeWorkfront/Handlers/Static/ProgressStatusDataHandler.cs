using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Handlers.Static;

public class ProgressStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new("LT", "Late"),
            new("ON", "On Time"),
            new("BH", "Behind"),
            new("RK", "At Risk")
        ];
    }
}