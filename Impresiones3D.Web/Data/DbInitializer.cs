using Impresiones3D.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Impresiones3D.Web.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Operador" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Usuario Admin
            var adminEmail = "admin@3dprintflow.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador"
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Usuario Operador
            var operadorEmail = "operador@3dprintflow.com";
            var operadorUser = await userManager.FindByEmailAsync(operadorEmail);
            if (operadorUser == null)
            {
                operadorUser = new ApplicationUser
                {
                    UserName = operadorEmail,
                    Email = operadorEmail,
                    NombreCompleto = "Operador"
                };
                await userManager.CreateAsync(operadorUser, "Operador123!");
                await userManager.AddToRoleAsync(operadorUser, "Operador");
            }
        }
    }
}