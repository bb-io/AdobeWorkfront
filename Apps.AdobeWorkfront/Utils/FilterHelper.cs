using System.Web;
using Apps.AdobeWorkfront.Constants;
using Apps.AdobeWorkfront.Models.Entities;
using Blackbird.Applications.Sdk.Common.Exceptions;

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
        return result.AddRangeFilterQueryParam(field, from?.ToString(DateTimeFormats.Fmt), to?.ToString(DateTimeFormats.Fmt));
    }
    
    public static List<QueryParameter> AddRangeFilter(
        this List<QueryParameter> result, 
        string field, 
        int? from, 
        int? to)
    {
        return result.AddRangeFilterQueryParam(field, from?.ToString(), to?.ToString());
    }

    public static List<QueryParameter> AddRawQuery(this List<QueryParameter> result, string? rawQuery)
    {
        if (string.IsNullOrWhiteSpace(rawQuery))
            return result;

        var parsed = HttpUtility.ParseQueryString(rawQuery);
        foreach (string? key in parsed.AllKeys)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new PluginMisconfigurationException("Incorrect query. Expected key=value pairs to be separated by &");
            
            foreach (var value in parsed.GetValues(key)!)
                result.Add(new QueryParameter(key, value));
        }

        return result;
    }
    
    private static List<QueryParameter> AddRangeFilterQueryParam(
        this List<QueryParameter> result, 
        string field, 
        string? from, 
        string? to)
    {
        switch (from, to)
        {
            case ({ } f, { } t):
                result.Add(new QueryParameter(field, f));
                result.Add(new QueryParameter($"{field}_Mod", "between"));
                result.Add(new QueryParameter($"{field}_Range", t));
                break;
            case ({ } f, null):
                result.Add(new QueryParameter(field, f));
                result.Add(new QueryParameter($"{field}_Mod", "gte"));
                break;
            case (null, { } t):
                result.Add(new QueryParameter(field, t));
                result.Add(new QueryParameter($"{field}_Mod", "lte"));
                break;
        }

        return result;
    }
}