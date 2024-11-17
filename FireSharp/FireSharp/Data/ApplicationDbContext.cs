using FireSharp.Constants;
using FireSharp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FireSharp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSeeding((context, b) =>
        {
            var permissions = AuthorizationServiceExtensions.GetAllPermissions<IPermission>().ToList();
            var role = new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Super Admin",
                NormalizedName = "SUPER ADMIN",
                Description = "Super Admin, has all permissions",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            ApplicationRole? superAdmin = context.Set<ApplicationRole>().FirstOrDefault(x => x.NormalizedName == role.NormalizedName);
            if (superAdmin == null)
            {
                context.Add(role);
                context.SaveChanges();
            }

            var latest = context.Set<IdentityRoleClaim<string>>().OrderBy(x => x.Id).LastOrDefault();
            var id = latest?.Id + 1 ?? 1;
            foreach (var perm in permissions)
            {
                
                
                var claim = new IdentityRoleClaim<string>
                {
                    Id = id,
                    RoleId = superAdmin?.Id ?? role.Id,
                    ClaimType = perm,
                    ClaimValue = perm
                };
                var roleClaim = context.Set<IdentityRoleClaim<string>>().FirstOrDefault(x => x.ClaimValue == claim.ClaimValue);
                if (roleClaim == null)
                {
                    context.Add(claim);
                    context.SaveChanges();
                    id++;
                }
                
            }
        });
    }

  
}