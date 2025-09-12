using System.Net;
using System.Text;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Apps.AdobeWorkfront.Utils;

public static class HeaderParametersExtensions
{
    public static string GetFileName(this IReadOnlyCollection<HeaderParameter> headers)
    {
        const string exceptionMessage = "It looks like API didn't return Content-Disposition header. Cannot determine the file name. Please contact blackbird support to resolve this issue.";
        
        var contentDisposition = headers
            .FirstOrDefault(x => x.Name != null && x.Name.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase))?.Value?.ToString();

        if (string.IsNullOrEmpty(contentDisposition))
        {
            throw new PluginApplicationException(exceptionMessage);
        }

        var parts = contentDisposition.Split(';')
            .Select(p => p.Trim())
            .ToList();

        var fileNameStarPart = parts.FirstOrDefault(p => p.StartsWith("filename*=", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(fileNameStarPart))
        {
            var encodedValue = fileNameStarPart.Split('=', 2).ElementAtOrDefault(1)?.Trim();
            if (!string.IsNullOrEmpty(encodedValue))
            {
                var sections = encodedValue.Split('\'');
                if (sections.Length == 3)
                {
                    var encoding = sections[0];
                    var encodedFileName = sections[2];

                    try
                    {
                        var bytes = WebUtility.UrlDecode(encodedFileName);
                        return Encoding.GetEncoding(encoding).GetString(Encoding.UTF8.GetBytes(bytes));
                    }
                    catch
                    {
                        return WebUtility.UrlDecode(encodedFileName);
                    }
                }
            }
        }

        var fileNamePart = parts.FirstOrDefault(p => p.StartsWith("filename=", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(fileNamePart))
        {
            var fileName = fileNamePart.Split('=', 2).ElementAtOrDefault(1)?.Trim().Trim('"');
            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }
        }
        
        throw new PluginApplicationException(exceptionMessage);
    }
}
