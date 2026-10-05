using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Entities;

namespace Apps.AdobeWorkfront.Utils;

public static class FilterHelper
{
    public static List<QueryParameter> AddEqualsFilter(this List<QueryParameter> result, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            result.Add(new QueryParameter(key, value));

        return result;
    }
    
    public static List<QueryParameter> AddContainsFilter(
        this List<QueryParameter> result, 
        string key, 
        string? value,
        bool caseSensitive = true)
    {
        if (string.IsNullOrWhiteSpace(value))
            return result;
        
        result.Add(new QueryParameter(key, value));
        result.Add(caseSensitive
            ? new QueryParameter($"{key}_Mod", "contains")
            : new QueryParameter($"{key}_Mod", "cicontains"));

        return result;
    }
    
    public static List<QueryParameter> AddRangeFilter(
        this List<QueryParameter> result, 
        string field, 
        DateTimeOffset? from, 
        DateTimeOffset? to)
    {
        switch (from, to)
        {
            case ({ } f, { } t):
                result.Add(new QueryParameter(field, f.ToString(DateTimeFormats.Fmt)));
                result.Add(new QueryParameter($"{field}_Mod", "between"));
                result.Add(new QueryParameter($"{field}_Range", t.ToString(DateTimeFormats.Fmt)));
                break;
            case ({ } f, null):
                result.Add(new QueryParameter(field, f.ToString(DateTimeFormats.Fmt)));
                result.Add(new QueryParameter($"{field}_Mod", "gte"));
                break;
            case (null, { } t):
                result.Add(new QueryParameter(field, t.ToString(DateTimeFormats.Fmt)));
                result.Add(new QueryParameter($"{field}_Mod", "lte"));
                break;
        }

        return result;
    }
}