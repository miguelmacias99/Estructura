using Plantilla.Dto.Modelo.Identity.Usuario;
using Plantilla.Entidad.Interfaz;
using Plantilla.Infraestructura.Extensiones.TipoDatos;
using Plantilla.Infraestructura.Modelo.Configuracion;
using Plantilla.Infraestructura.Modelo.Correo;
using Plantilla.Infraestructura.Modelo.Respuestas;
using Plantilla.Infraestructura.Services.Claves;
using Plantilla.Infraestructura.Services.Correo;
using Plantilla.Infraestructura.Services.Razor;
using Plantilla.Infraestructura.Services.Sesion;
using Plantilla.Infraestructura.Utilidades.Generico;
using Plantilla.Infraestructura.Utilidades.Logger;
using Plantilla.Infraestructura.Utilidades.Mapeador;
using Plantilla.RepositorioEfCore.Extensiones;
using Microsoft.Extensions.Options;
using static Plantilla.Dto.Modelo.Identity.Usuario.UsuarioDto;

namespace Plantilla.Negocio.Identity.Usuario
{
    internal class NeUsuario(IUnidadTrabajo unidadTrabajo, ISesionService sesionService,
        IOptions<UrlIdentity> urlIdentity, IEnvioCorreoService envioCorreoService,
        IHtmlRenderService renderService, IClaveAleatoriaService claveAleatoriaService) : INeUsuario
    {
        private readonly UrlIdentity urlIdentity = urlIdentity.Value;

        public async Task<RespuestaGenericaDto> Actualizar(ActualizarUsuario actualizar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(actualizar);
                if (!esValido) return respuesta;

                // Procesamos
                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorId(actualizar.Id!.Value);

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el usuario a actualizar");

                // verificamos que el nuevo 2FA no sea "Ninguno" y que se está realizando un cambio a un nuevo 2FA
                var actualizarToken2FA = actualizar.Tipo2FA != TwoFactorTypeDto.None
                    && usuario.Tipo2FA != TwoFactorTypeExtensions.ObtenerDesdeNumero((int)actualizar.Tipo2FA);

                usuario.FirstName = actualizar.FirstName!;
                usuario.LastName = actualizar.LastName!;
                usuario.IsSuperUser = actualizar.IsSuperUser;
                usuario.PhoneNumber = actualizar.PhoneNumber!;
                usuario.Tipo2FA = TwoFactorTypeExtensions.ObtenerDesdeNumero((int)actualizar.Tipo2FA);
                usuario.TwoFactorEnabled = actualizar.Tipo2FA != TwoFactorTypeDto.None;
                usuario.UsuarioModificacion = sesionService.ObtenerUsuarioActual();
                usuario.FechaModificacion = DateTime.Now;
                usuario.IpModificacion = sesionService.ObtenerIpCliente();

                await unidadTrabajo.Usuarios.Actualizar(usuario, actualizarToken2FA);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, actualizar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> CambiarClave(CambiarClaveUsuario cambiar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(cambiar);
                if (!esValido) return respuesta;

                // Procesamos
                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorId(cambiar.Id!.Value);

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el usuario a eliminar");

                var errores = await unidadTrabajo
                    .Usuarios
                    .CambiarClaveUsuario(usuario, cambiar.CurrentPasswordHash!, cambiar.NewPasswordHash!);
                if (!string.IsNullOrEmpty(errores)) return RespuestaGenericaDto.ErrorComun(errores);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, cambiar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaConsultaDto<UsuarioDto>> ConsultarPorId(ConsultarUsuario consultar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(consultar);
                if (!esValido) return new(respuesta);

                // Procesamos
                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorId(consultar.Id!.Value);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = usuario?.Mapear<UsuarioDto>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, consultar);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaConsultasDto<UsuarioDto>> ConsultarTodos(ConsultarUsuarios consultar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(consultar);
                if (!esValido) return new(respuesta);

                // Procesamos
                var usuarios = await unidadTrabajo
                    .Usuarios
                    .ConsultarTodos(consultar.Paginacion);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultados = usuarios?.Mapear<List<UsuarioDto>>(),
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaDto> Crear(CrearUsuario crear)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(crear);
                if (!esValido) return respuesta;

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(crear.Email!.ToNormalize());

                if (usuario is not null) return RespuestaGenericaDto.ErrorComun("Ya existe un usuario registrado con el correo indicado");

                await unidadTrabajo
                    .Usuarios
                    .Crear(new()
                    {
                        FirstName = crear.FirstName!,
                        LastName = crear.LastName!,
                        Email = crear.Email!,
                        NormalizedEmail = crear.Email!.ToNormalize(),
                        AccessFailedCount = 3,
                        EmailConfirmed = false,
                        IsSuperUser = crear.IsSuperUser,
                        LockoutEnabled = false,
                        UserName = crear.Email!,
                        NormalizedUserName = crear.Email!.ToNormalize(),
                        PasswordHash = crear.PasswordHash!,
                        PhoneNumber = crear.PhoneNumber!,
                        PhoneNumberConfirmed = false,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Tipo2FA = TwoFactorTypeExtensions.ObtenerDesdeNumero((int)crear.Tipo2FA),
                        TwoFactorEnabled = crear.Tipo2FA != TwoFactorTypeDto.None,
                        Activo = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = sesionService.ObtenerUsuarioActual(),
                        IpRegistro = sesionService.ObtenerIpCliente(),
                        FechaModificacion = DateTime.Now,
                        UsuarioModificacion = sesionService.ObtenerUsuarioActual(),
                        IpModificacion = sesionService.ObtenerIpCliente(),
                    });

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, crear);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> Desbloquear(DesbloquearUsuario desbloquear)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(desbloquear);
                if (!esValido) return respuesta;

                // Procesamos
                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorId(desbloquear.Id!.Value);

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el usuario a desbloquear");
                if (!await unidadTrabajo.Usuarios.VerificarUsuarioBloqueado(usuario)) return RespuestaGenericaDto.ErrorComun("El usuario indicado no se encuentra bloqueado");

                // Se desbloquea el usuario
                var errores = await unidadTrabajo.Usuarios.DesbloquearUsuario(usuario);
                if (!string.IsNullOrEmpty(errores)) return RespuestaGenericaDto.ErrorComun(errores);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, desbloquear);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> Eliminar(EliminarUsuario eliminar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(eliminar);
                if (!esValido) return respuesta;

                // Procesamos
                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorId(eliminar.Id!.Value);

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("No se ha encontrado el usuario a eliminar");

                usuario.Activo = false;
                usuario.UsuarioModificacion = sesionService.ObtenerUsuarioActual();
                usuario.FechaModificacion = DateTime.Now;
                usuario.IpModificacion = sesionService.ObtenerIpCliente();

                await unidadTrabajo.Usuarios.Actualizar(usuario);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, eliminar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> EnviarConfirmacionCorreo(EnviarConfirmacionCorreoUsuario enviar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(enviar);
                if (!esValido) return respuesta;

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(enviar.Email!.ToNormalize());

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("El usuario no existe");
                if (usuario.EmailConfirmed) return RespuestaGenericaDto.ErrorComun("El usuario ya ha confirmado el correo");

                var tokenConfirmacion = await unidadTrabajo.Usuarios.ObtenerTokenConfirmacionCorreo(usuario);
                var confirmationLink = $"{urlIdentity.UrlConfirmacionCorreo}?id={usuario.Id}&token={Uri.EscapeDataString(tokenConfirmacion)}";

                var respuestaEnvioCorreo = await envioCorreoService.EnviarCorreo(new(usuario.Email!)
                {
                    Asunto = "Confirmación de Cuenta",
                    Cuerpo = await renderService.RenderizarVistaModelAsync("Views/Correo/PlantillaCorreo.cshtml", new PlantillaCorreoModel()
                    {
                        Titulo = "Confirma tu correo electrónico",
                        Mensaje = "Utiliza el siguiente Link para confirmar el correo electrónico",
                        LinkAccion = confirmationLink,
                    }),
                });

                return respuestaEnvioCorreo;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, enviar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaDto> EnviarCorreoClaveOlvidada(EnviarCorreoClaveOlvidadaUsuario enviar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(enviar);
                if (!esValido) return respuesta;

                var usuario = await unidadTrabajo
                    .Usuarios
                    .ConsultarPorCorreoNormalizado(enviar.Email!.ToNormalize());

                if (usuario is null) return RespuestaGenericaDto.ErrorComun("El usuario no existe");

                var tokenConfirmacion = await unidadTrabajo.Usuarios.ObtenerTokenClaveOlvidada(usuario);
                var confirmationLink = $"{urlIdentity.UrlReestablecerClaveUsuario}?id={usuario.Id}&token={Uri.EscapeDataString(tokenConfirmacion)}";

                var respuestaEnvioCorreo = await envioCorreoService.EnviarCorreo(new(usuario.Email!)
                {
                    Asunto = "Reestablecer Credenciales de Acceso",
                    Cuerpo = await renderService.RenderizarVistaModelAsync("Views/Correo/PlantillaCorreo.cshtml", new PlantillaCorreoModel()
                    {
                        Titulo = "Reestablecer Credenciales",
                        Mensaje = "Utiliza el siguiente Link para reestablecer credenciales",
                        LinkAccion = confirmationLink,
                    }),
                });

                return respuestaEnvioCorreo;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, enviar);
                return RespuestaGenericaDto.Excepcion();
            }
        }

        public async Task<RespuestaGenericaConsultaDto<ClaveReestablecidaUsuarioDto>> ReestablecerClaveOlvidada(ReestablecerClaveUsuario reestablecer)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(reestablecer);
                if (!esValido) return new(respuesta);

                var contrasenaNueva = claveAleatoriaService.GenerarClaveAleatorea();
                await unidadTrabajo
                    .Usuarios
                    .CambiarClaveToken(reestablecer.Id!.Value, reestablecer.Token!, contrasenaNueva);

                return new()
                {
                    Respuesta = RespuestaGenericaDto.ExitoComun(),
                    Resultado = new()
                    {
                        CredencialTemporal = contrasenaNueva,
                    }
                };
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, reestablecer);
                return new(RespuestaGenericaDto.Excepcion());
            }
        }

        public async Task<RespuestaGenericaDto> ValidarConfirmacionCorreo(ValidarConfirmacionCorreoUsuario validar)
        {
            try
            {
                // Validamos la entrada al servicio
                var (respuesta, esValido) = ValidarDataAnnotation.Validar(validar);
                if (!esValido) return respuesta;

                await unidadTrabajo
                    .Usuarios
                    .ValidarTokenConfirmacionCorreo(validar.Id!.Value, validar.Token!);

                return RespuestaGenericaDto.ExitoComun();
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex, validar);
                return RespuestaGenericaDto.Excepcion();
            }
        }
    }
}