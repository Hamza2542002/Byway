namespace Byway.Core.Helpers;

public class JWTConfiguration
{
    public string? SecurityKey { get; set; }
    public string? AudienceIP { get; set; }
    public string? IssuerIP { get; set; }
    public double DurationInDays { get; set; }
}
