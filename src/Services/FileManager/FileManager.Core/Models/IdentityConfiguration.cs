namespace FileManager.Core.Models;

public class IdentityConfiguration
{
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
}
