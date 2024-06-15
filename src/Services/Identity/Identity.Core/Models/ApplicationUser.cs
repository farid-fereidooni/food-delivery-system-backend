using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public required string? FirstName { get; set; }
    public required string? LastName { get; set; }
    public string FullName => string.Join(' ', FirstName, LastName).Trim();
}
