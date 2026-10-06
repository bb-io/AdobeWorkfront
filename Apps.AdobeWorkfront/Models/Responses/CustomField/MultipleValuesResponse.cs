using Blackbird.Applications.Sdk.Common;

namespace Apps.AdobeWorkfront.Models.Responses.CustomField;

public record MultipleValuesResponse([property: Display("Custom field values")] List<string> CustomFieldValues);