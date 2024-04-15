using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.WebApi.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task SeedRoles(this IServiceProvider app)
    {
        var roleManager = app.GetRequiredService<RoleManager<ApplicationRole>>();

        var roles = typeof(UserRole).GetFields(BindingFlags.Public | BindingFlags.Static |
           BindingFlags.FlattenHierarchy).Select(x => (string)x.GetValue(null)!);

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }
    }

    public static async Task SeedAdmin(this IServiceProvider app)
    {
        var userManager = app.GetRequiredService<UserManager<ApplicationUser>>();

        const string USERNAME = "myadmin@myadmin.com";
        const string PASSWORD = "Admin1@";

        var existingUser = await userManager.FindByNameAsync(USERNAME);

        if (existingUser == null)
        {
            var user = new ApplicationUser
            {
                UserName = USERNAME,
                Email = USERNAME
            };

            var result = await userManager.CreateAsync(user, PASSWORD);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, UserRole.ADMIN);
            }
        }
    }
}
