using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Infraestructura.Modelo.Consulta;

namespace Plantilla.Entidad.Interfaz.Identity
{
    public interface IUsuarioRepositorio
    {
        Task<long> Crear(AuthUser entidad);

        Task Actualizar(AuthUser entidad, bool actualizarToken2FA = false);

        Task<AuthUser?> ConsultarPorId(long id);

        Task<AuthUser?> ConsultarPorCorreoNormalizado(string correo);

        Task<IEnumerable<AuthUser>> ConsultarTodos(Paginacion paginacion);

        Task<string> ObtenerTokenConfirmacionCorreo(AuthUser entidad);

        Task ValidarTokenConfirmacionCorreo(long id, string token);

        Task<string> CambiarClaveUsuario(AuthUser entidad, string claveActual, string claveNueva);

        Task CambiarClaveToken(long id, string token, string claveNueva);

        Task<string> ObtenerTokenClaveOlvidada(AuthUser entidad);

        Task<bool> VerificarUsuarioBloqueado(AuthUser entidad);

        Task<string> DesbloquearUsuario(AuthUser entidad);

        Task<string?> GenerarTokenFactorAutenticacion(AuthUser entidad, string proovedorToken);

        Task<bool> ValidarTokenFactorAutenticacion(AuthUser entidad, string proovedorToken, string token);

        Task<string> ObtenerAuthenticatorKey(AuthUser entidad);
    }
}