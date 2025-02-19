using Cititrans.Auth.Entidad.Modelo.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cititrans.Auth.Entidad.Contextos
{
    public class CititransDbContext(DbContextOptions<CititransDbContext> options) : IdentityDbContext<AuthUser, AuthRol, long>(options)
    {
    }
}