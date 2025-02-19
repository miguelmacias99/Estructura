using Plantilla.Dto.Modelo.Identity.Login;
using Plantilla.Entidad.Interfaz;
using Plantilla.Entidad.Modelo.Identity;
using Plantilla.Infraestructura.Extensiones.TipoDatos;
using Plantilla.Infraestructura.Modelo.Correo;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Services.Correo;
using Plantilla.Infraestructura.Services.Razor;
using Plantilla.Infraestructura.Services.Tokens;
using Plantilla.Infraestructura.Utilidades.Generico;
using Plantilla.Infraestructura.Utilidades.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Plantilla.Negocio.Identity.Login
{
    internal class NeLogin(IUnidadTrabajo unidadTrabajo, IJwtService jwtService,
        SignInManager<AuthUser> signInManager, IEnvioCorreoService envioCorreoService,
        IHtmlRenderService renderService, IQrService qrService) : INeLogin
    {
        public Task<RespuestaGenericaDto> CerrarSesion()
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> IniciarSesion(LoginDto login)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(login);
                if (!esValido) return new(respuesta);

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(login.Usuario!.ToNormalize());
                if (usuario is null) return new(RespuestaGenericaDto.ErrorComun("Credenciales incorrectas"));

                var resultadoInicioSesion = await signInManager
                    .PasswordSignInAsync(usuario, login.Clave!, login.RecordarSesion, lockoutOnFailure: true);

                if (resultadoInicioSesion.IsLockedOut)
                    return new(RespuestaGenericaDto.ErrorComun("Cuenta bloqueada temporalmente."));

                if (resultadoInicioSesion.RequiresTwoFactor)
                {
                    // Se procesa el envio del 2fa, si falla, se envia el error
                    var respuesta2Fa = await ProcesarEnvioFactorAutenticacion(usuario);
                    if (!respuesta2Fa.EsExitosa) return new(respuesta2Fa);

                    // Validar 2AT
                    return new(RespuestaGenericaDto.ExitoComun())
                    {
                        Resultado = new(true)
                    };
                }

                if (!resultadoInicioSesion.Succeeded)
                    return new(RespuestaGenericaDto.ErrorComun("Usuario o contraseña incorrectos."));

                // Generar el token y datos del usuario
                var jwtData = jwtService.GenerarToken([
                    new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new(JwtRegisteredClaimNames.Email, usuario.Email!.ToString()),
                    new(JwtRegisteredClaimNames.Name, usuario.ObtenerNombres()),
                ]);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = new()
                    {
                        Jwt = jwtData,
                        ValidarFactorAutenticacion = false
                    }
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, login);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaConsultaDto<ResultadoLoginDto>> ValidarFactorAutenticacion(LoginFactorAutenticacionDto login)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(login);
                if (!esValido) return new(respuesta);

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(login.Usuario!.ToNormalize());

                if (usuario is null)
                {
                    LogUtils.LogError(new InvalidOperationException("El usuario no ha sido encontrado"), login);
                    return new(RespuestaGenericaDto.ErrorComun("No se ha podido procesar la validacion de los factores de autenticacion"));
                }

                var respuestaValidacion = await ValidarFactorAutenticacion(usuario, login.CodigoFactorAutenticacion!);
                if (!respuestaValidacion.EsExitosa)
                    return new(respuestaValidacion);

                // Generar el token y datos del usuario
                var jwtData = jwtService.GenerarToken([
                    new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new(JwtRegisteredClaimNames.Email, usuario.Email!.ToString()),
                    new(JwtRegisteredClaimNames.Name, usuario.ObtenerNombres()),
                ]);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = new()
                    {
                        Jwt = jwtData,
                        ValidarFactorAutenticacion = false
                    }
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, login);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaConsultaDto<ResultadoQrDto>> GenerarCodigoQrAutenticathor(GenerarQrAutenticadorUsuarioDto generar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(generar);
                if (!esValido) return new(respuesta);

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(generar.Usuario!.ToNormalize());
                if (usuario is null) return new(RespuestaGenericaDto.ErrorComun("El usuario indicado no es válido"));
                if (!usuario.Tiene2AFAuthenticatorEnabled()) return new(RespuestaGenericaDto.ErrorComun("El usuario no está configurado para generar códigos Qr"));

                var authKey = await unidadTrabajo.Usuarios.ObtenerAuthenticatorKey(usuario);
                var respuestaQr = await qrService.GenerarCodigoQrAuthenticator(new()
                {
                    AuthenticatorKey = authKey,
                    Correo = usuario.Email!,
                });

                if (!respuestaQr.Respuesta.EsExitosa) return new(respuestaQr.Respuesta);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = new()
                    {
                        CodigoQrBase64 = respuestaQr.Resultado!.Base64,
                        CodigoQrUri = respuestaQr.Resultado!.Uri
                    }
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, generar);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        #region Métodos para generacion y validacion de 2FA

        private async Task<RespuestaGenericaDto> ValidarFactorAutenticacion(AuthUser usuario, string token)
        {
            var mensajeError = usuario.Tipo2FA switch
            {
                TwoFactorType.None => "Usuario no requiere Factor de Autenticación, existe un error de configuración",
                TwoFactorType.Email => await EjecutarValidacionFactorAutenticacion(usuario, TokenOptions.DefaultEmailProvider, token),
                TwoFactorType.Authenticator => await EjecutarValidacionFactorAutenticacion(usuario, TokenOptions.DefaultAuthenticatorProvider, token),
                _ => "Factor de Autenticación no implementado"
            };

            return string.IsNullOrEmpty(mensajeError)
                ? RespuestaGenericaDto.ExitoComun()
                : RespuestaGenericaDto.ErrorComun(mensajeError);
        }

        private async Task<string> EjecutarValidacionFactorAutenticacion(AuthUser usuario, string proveedor, string token)
        {
            return await unidadTrabajo.Usuarios.ValidarTokenFactorAutenticacion(usuario, proveedor, token)
                ? string.Empty
                : "Autenticación Fallida. El token es inválido";
        }

        private async Task<RespuestaGenericaDto> ProcesarEnvioFactorAutenticacion(AuthUser usuario)
        {
            return usuario.Tipo2FA switch
            {
                TwoFactorType.None => RespuestaGenericaDto.ErrorComun("Usuario no requiere Factor de Autenticación, existe un error de configuración"),
                TwoFactorType.Email => await GenerarToken2FACorreo(usuario),
                TwoFactorType.Authenticator => await GenerarToken2FAAuthenticator(usuario),
                _ => RespuestaGenericaDto.ErrorComun("Factor de Autenticación no implementado")
            };
        }

        private async Task<RespuestaGenericaDto> GenerarToken2FAAuthenticator(AuthUser usuario)
        {
            await unidadTrabajo.Usuarios.GenerarTokenFactorAutenticacion(usuario, TokenOptions.DefaultAuthenticatorProvider);
            return RespuestaGenericaDto.ExitoComun();
        }

        private async Task<RespuestaGenericaDto> GenerarToken2FACorreo(AuthUser usuario)
        {
            var token = await unidadTrabajo.Usuarios.GenerarTokenFactorAutenticacion(usuario, TokenOptions.DefaultEmailProvider);
            if (string.IsNullOrEmpty(token)) return RespuestaGenericaDto.ErrorComun("No se ha podido obtener un token de factor de autenticación");
            return await envioCorreoService.EnviarCorreo(new(usuario.Email!)
            {
                Asunto = "Confirma tu Inicio de Sesión",
                Cuerpo = await renderService.RenderizarVistaModelAsync("Views/Correo/PlantillaCorreo.cshtml", new PlantillaCorreoModel()
                {
                    Titulo = "Confirma tu inicio de sesión",
                    Mensaje = "Utiliza el siguiente código para confirmar el acceso al sistema",
                    Codigo = token,
                }),
            });
        }

        #endregion Métodos para generacion y validacion de 2FA
    }
}