using System.Reflection;
using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Entidad.Modelo.Procesos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Plantilla.Entidad.Contextos
{
    public class CititransDbContext(DbContextOptions<CititransDbContext> options) : IdentityDbContext<AuthUser, AuthRol, long>(options)
    {
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}