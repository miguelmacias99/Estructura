using Cititrans.Auth.Entidad.Modelo.Identity;
using Cititrans.Auth.Infraestructura.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Cititrans.Auth.RepositorioEfCore.CargaInicial
{
    public static class IdentitySeeder
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AuthRol>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthUser>>();

            // Definir roles
            string[] roles = ["Administrador", "Usuario"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AuthRol()
                    {
                        Activo = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        Name = role,
                        FechaModificacion = DateTime.Today,
                        FechaRegistro = DateTime.Today,
                        UsuarioModificacion = UsuarioSistemaConstante.UsuarioRegistro,
                        UsuarioRegistro = UsuarioSistemaConstante.UsuarioRegistro,
                        IpModificacion = "::1",
                        IpRegistro = "::1",
                        NormalizedName = role,
                    });
                }
            }

            // Crear usuario "Admin"
            var adminEmail = "admin@apptelink.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AuthUser
                {
                    FirstName = "Admin",
                    LastName = string.Empty,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Activo = true,
                    FechaModificacion = DateTime.Today,
                    FechaRegistro = DateTime.Today,
                    UsuarioModificacion = UsuarioSistemaConstante.UsuarioRegistro,
                    UsuarioRegistro = UsuarioSistemaConstante.UsuarioRegistro,
                    IpModificacion = "::1",
                    IpRegistro = "::1",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Administrador");
                }
            }
        }
    }
}