using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Models;

public class ApplicationUser : IdentityUser<int>
{
    public virtual Employee Employee { get; set; }
}