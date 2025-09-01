using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.AdobeWorkfront.Handlers.Static;

public class ProjectStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new("CUR", "Current"),
            new("ONH", "On Hold"),
            new("PLN", "Planning"),
            new("CPL", "Complete"),
            new("DED", "Dead"),
            new("REQ", "Requested"),
            new("APR", "Approved"),
            new("REJ", "Rejected"),
            new("IDA", "Idea")
        ];
    }
}