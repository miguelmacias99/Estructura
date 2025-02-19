using Cititrans.Auth.Entidad.Interfaz.Identity;

namespace Cititrans.Auth.Entidad.Interfaz
{
    public interface IUnidadTrabajo
    {
        public IRolRepositorio Roles { get; }
        public IUsuarioRepositorio Usuarios { get; }

        void Begin();

        void Commit();

        void Rollback();
    }
}