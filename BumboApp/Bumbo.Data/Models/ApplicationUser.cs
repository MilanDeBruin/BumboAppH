using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Models;

public class ApplicationUser : IdentityUser
{
    public virtual Employee Employee { get; set; }
}