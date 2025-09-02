using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.AdobeWorkfront.Models.Responses;

public class UserResponse
{
    [Display("User ID"), JsonProperty("ID")]
    public string UserId { get; set; } = string.Empty;
    
    [Display("Username"), JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Object code"), JsonProperty("objCode")]
    public string ObjCode { get; set; } = string.Empty;
    
    [Display("Email address"), JsonProperty("emailAddr")]
    public string EmailAddr { get; set; } = string.Empty;
    
    [Display("License type"), JsonProperty("licenseType")]
    public string LicenseType { get; set; } = string.Empty;
    
    [Display("Work hours per day"), JsonProperty("workHoursPerDay")]
    public double WorkHoursPerDay { get; set; }
}