using Microsoft.AspNetCore.Identity;

namespace FireSharp.Data;

public class ApplicationRole: IdentityRole
{
    
    public string? Description { get; set; }
}