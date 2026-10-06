using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses.CustomField;

public record DateResponse([property: Display("Custom field value")] DateTime? CustomFieldValue);