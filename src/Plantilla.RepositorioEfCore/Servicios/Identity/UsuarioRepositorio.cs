using Plantilla.Entidad.Contextos;
using Plantilla.Entidad.Interfaz.Identity;
using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Infraestructura.Modelo.Consulta;
using Microsoft.AspNetCore.Identity;
using X.PagedList.Extensions;

namespace Plantilla.RepositorioEfCore.Servicios.Identity
{
    internal class UsuarioRepositorio(UserManager<AuthUser> usuarioManager, CititransDbContext context) : IUsuarioRepositorio
    {
        private readonly UserManager<AuthUser> _usuarioManager = usuarioManager;

        public async Task Actualizar(AuthUser entidad, bool actualizarToken2FA = false)
        {
            // Usamos la transaccion para asegurar integridad de datos
            await using var transaction = await context.Database.BeginTransactionAsync();

            var resultado = await _usuarioManager.UpdateAsync(entidad);
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }

            if (actualizarToken2FA)
            {
                resultado = await _usuarioManager.ResetAuthenticatorKeyAsync(entidad);
                if (!resultado.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
                }
            }

            await transaction.CommitAsync();
        }

        public async Task<AuthUser?> ConsultarPorCorreoNormalizado(string correo)
        {
            return await _usuarioManager.FindByEmailAsync(correo);
        }

        public async Task<AuthUser?> ConsultarPorId(long id)
        {
            return await _usuarioManager.FindByIdAsync(id.ToString());
        }

        public async Task<IEnumerable<AuthUser>> ConsultarTodos(Paginacion paginacion)
        {
            var usuariosPaginados = _usuarioManager.Users.ToPagedList(paginacion.PageNumber, paginacion.Take);
            return await Task.FromResult(usuariosPaginados.ToList());
        }

        public async Task<string> ObtenerTokenConfirmacionCorreo(AuthUser entidad)
        {
            var token = await _usuarioManager.GenerateEmailConfirmationTokenAsync(entidad);
            if (string.IsNullOrEmpty(token)) throw new InvalidOperationException("No se pudo generar un token de confirmación");

            return token;
        }

        public async Task ValidarTokenConfirmacionCorreo(long id, string token)
        {
            var entidad = await ConsultarPorId(id)
                ?? throw new InvalidOperationException("La entidad no ha podido ser recuperada");
            await _usuarioManager.ConfirmEmailAsync(entidad, token);
        }

        public async Task<long> Crear(AuthUser entidad)
        {
            // Usamos la transaccion para asegurar integridad de datos
            await using var transaction = await context.Database.BeginTransactionAsync();

            var resultado = await _usuarioManager.CreateAsync(entidad, entidad.PasswordHash!);
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }

            if (entidad.Tipo2FA != TwoFactorType.None)
            {
                resultado = await _usuarioManager.ResetAuthenticatorKeyAsync(entidad);
                if (!resultado.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
                }
            }
            await transaction.CommitAsync();

            return entidad.Id;
        }

        public async Task<string> ObtenerTokenClaveOlvidada(AuthUser entidad)
        {
            var token = await _usuarioManager.GeneratePasswordResetTokenAsync(entidad);
            if (string.IsNullOrEmpty(token)) throw new InvalidOperationException("No se pudo generar un token de reestablecimiento de clave");

            return token;
        }

        public async Task CambiarClaveToken(long id, string token, string claveNueva)
        {
            var entidad = await ConsultarPorId(id)
                ?? throw new InvalidOperationException("La entidad no ha podido ser recuperada");
            await _usuarioManager.ConfirmEmailAsync(entidad, token);

            var resultado = await _usuarioManager.ResetPasswordAsync(entidad, token, claveNueva);
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }
        }

        public async Task<string> CambiarClaveUsuario(AuthUser entidad, string claveActual, string claveNueva)
        {
            var resultado = await _usuarioManager.ChangePasswordAsync(entidad, claveActual, claveNueva);
            if (!resultado.Succeeded)
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }

            return string.Empty;
        }

        public async Task<bool> VerificarUsuarioBloqueado(AuthUser entidad)
        {
            return await _usuarioManager.IsLockedOutAsync(entidad);
        }

        public async Task<string> DesbloquearUsuario(AuthUser entidad)
        {
            var resultado = await _usuarioManager.SetLockoutEndDateAsync(entidad, DateTimeOffset.UtcNow);
            if (!resultado.Succeeded)
            {
                return string.Join(", ", resultado.Errors.Select(e => e.Description));
            }

            return string.Empty;
        }

        public async Task<string?> GenerarTokenFactorAutenticacion(AuthUser entidad, string proovedorToken)
        {
            var token = await _usuarioManager.GenerateTwoFactorTokenAsync(entidad, proovedorToken);
            return token;
        }

        public async Task<bool> ValidarTokenFactorAutenticacion(AuthUser entidad, string proovedorToken, string token)
        {
            return await _usuarioManager.VerifyTwoFactorTokenAsync(entidad, proovedorToken, token);
        }

        public async Task<string> ObtenerAuthenticatorKey(AuthUser entidad)
        {
            var authKey = await _usuarioManager.GetAuthenticatorKeyAsync(entidad);
            if (string.IsNullOrEmpty(authKey)) throw new InvalidOperationException("No se pudo obtener la llave de autenticación del usuario");

            return authKey;
        }
    }
}