using System.Reflection;
using FireSharp.Data;
using FireSharp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FireSharp.Constants;



public static class AuthorizationServiceExtensions
{
    public static IServiceCollection RegisterPermissionClaims(this IServiceCollection services)
    {
        var permissions = GetAllFilesWithInterface<IPermission>();

        services.AddAuthorization(opt =>
        {
            foreach (var perm in permissions)
            {
                foreach (var claim in perm.GetFields( BindingFlags.Public)
                             .Where(f => f.IsLiteral && !f.IsInitOnly))
                {
                    var permission = claim.GetValue(null);
                    if(permission?.ToString() is { Length: > 0 })
                    {
                        opt.AddPolicy(nameof(permission), policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.RequireClaim(permission.ToString());
                        });
                    }
                }
            }
        });
        
        return services;
    }

    public static IEnumerable<Type> GetAllFilesWithInterface<T>()
    {
        var interfaceType = typeof(T);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var files = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(interfaceType))
            .ToList();
        return files;
    }

    public static IEnumerable<string> GetAllPermissions<T>()
    {
        var interfaceType = typeof(T);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var files = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(interfaceType))
            .ToList();

        List<string> permissions = [];
        
        foreach (var perm in files)
        {
            foreach (var claim in perm.GetFields( BindingFlags.Public | BindingFlags.Static)
                         .Where(f => f.IsLiteral && !f.IsInitOnly))
            {
                var permission = claim.GetValue(null);
                if(permission?.ToString() is { Length: > 0 })
                {
                    permissions.Add(permission.ToString()!);
                }
            }
        }
        return permissions;
    }
}