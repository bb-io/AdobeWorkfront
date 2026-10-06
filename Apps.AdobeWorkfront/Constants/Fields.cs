namespace Apps.AdobeWorkfront.Constants;

public static class Fields
{
    private static readonly string[] TaskFieldNames = new[]
    {
        "percentComplete",
        "plannedCompletionDate", 
        "plannedStartDate", 
        "priority", 
        "progressStatus", 
        "projectedCompletionDate", 
        "projectedStartDate", 
        "status", 
        "taskNumber", 
        "wbs", 
        "assignmentsListString", 
        "assignedToID",
        "parentID",
        "parent:name", 
        "description", 
        "projectID",
        "project:name",
        "documents:ID", 
        "documents:name", 
        "documents:currentVersion:ext", 
        "enteredByID", 
        "enteredBy:name", 
        "enteredBy:emailAddr",
        "parameterValues:*"
    };

    public static readonly string TaskFields = string.Join(',', TaskFieldNames);
}