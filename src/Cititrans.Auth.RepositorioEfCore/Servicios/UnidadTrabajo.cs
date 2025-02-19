using System.Data;
using Cititrans.Auth.Entidad.Contextos;
using Cititrans.Auth.Entidad.Interfaz;
using Cititrans.Auth.Entidad.Interfaz.Identity;
using Cititrans.Auth.Entidad.Modelo.Identity;
using Cititrans.Auth.RepositorioEfCore.Servicios.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cititrans.Auth.RepositorioEfCore.Servicios
{
    internal class UnidadTrabajo(IDbConnection connection,
        RoleManager<AuthRol> roleManager, UserManager<AuthUser> userManager,
        CititransDbContext context) : IUnidadTrabajo
    {
        private readonly IDbConnection _connection = connection;
        private IDbTransaction? _transaction;

        public IRolRepositorio Roles => new RolRepositorio(roleManager);
        public IUsuarioRepositorio Usuarios => new UsuarioRepositorio(userManager, context);

        public void Begin()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción en curso.");

            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
                _connection.Close();
            }
        }

        public void Rollback()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
                _connection.Close();
            }
        }
    }
}