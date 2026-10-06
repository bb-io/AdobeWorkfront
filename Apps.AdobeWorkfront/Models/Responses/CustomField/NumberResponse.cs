using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses.CustomField;

public record NumberResponse([property: Display("Custom field value")] decimal? CustomFieldValue);