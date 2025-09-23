using Microsoft.AspNetCore.Identity;

namespace Byway.Core.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? ImageUrl { get; set; }
}
