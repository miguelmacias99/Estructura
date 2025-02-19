using Plantilla.Entidad.Interfaz.Identity;
using Plantilla.Entidad.Modelo.Identity;
using Microsoft.AspNetCore.Identity;

namespace Plantilla.RepositorioEfCore.Servicios.Identity
{
    internal class RolRepositorio(RoleManager<AuthRol> rolManager) : IRolRepositorio
    {
        private readonly RoleManager<AuthRol> _rolManager = rolManager;

        public async Task Actualizar(AuthRol rol)
        {
            await _rolManager.UpdateAsync(rol);
        }

        public async Task<AuthRol?> ConsultarPorId(long id)
        {
            return await _rolManager.FindByIdAsync(id.ToString());
        }

        public async Task<AuthRol?> ConsultarPorNombre(string nombre)
        {
            return await _rolManager.FindByNameAsync(nombre);
        }

        public async Task<IEnumerable<AuthRol>> ConsultarTodos()
        {
            return await Task.FromResult(_rolManager.Roles.ToList());
        }

        public async Task<long> Crear(AuthRol rol)
        {
            var resultado = await _rolManager.CreateAsync(rol);
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }
            return rol.Id;
        }
    }
}