using Cititrans.Auth.Entidad.Modelo.Identity;

namespace Cititrans.Auth.Entidad.Interfaz.Identity
{
    public interface IRolRepositorio
    {
        Task<long> Crear(AuthRol rol);

        Task Actualizar(AuthRol rol);

        Task<AuthRol?> ConsultarPorNombre(string nombre);

        Task<AuthRol?> ConsultarPorId(long id);

        Task<IEnumerable<AuthRol>> ConsultarTodos();
    }
}