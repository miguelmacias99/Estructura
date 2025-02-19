using Plantilla.Entidad.Interfaz.Identity;
using Plantilla.Entidad.Interfaz.Procesos;

namespace Plantilla.Entidad.Interfaz
{
    public interface IUnidadTrabajo
    {
        public IRolRepositorio Roles { get; }
        public IUsuarioRepositorio Usuarios { get; }
        public ITaskItemRepositorio TaskItems { get; }

        void Begin();

        void Commit();

        void Rollback();
    }
}