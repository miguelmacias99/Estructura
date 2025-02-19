using System.Data;
using Plantilla.Infraestructura.Extensiones.Collections;
using Dapper;

namespace Plantilla.Infraestructura.Extensiones.Dapper
{
    public static class DynamicParameterExtensions
    {
        #region Parametros de entrada

        public static DynamicParameters ParametroEntrada<T>(this DynamicParameters parametros, string nombreParametro, string nombreTipo, ICollection<T> valor)
        {
            var dtTipo = valor.ConvertirADataTable();
            parametros.Add(nombreParametro, dtTipo.AsTableValuedParameter(nombreTipo), direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, decimal? valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.Decimal, direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, string valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.String, direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, long? valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.Int64, direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, bool? valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.Boolean, direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, DateTime? valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.DateTime, direction: ParameterDirection.Input);

            return parametros;
        }

        public static DynamicParameters ParametroEntrada(this DynamicParameters parametros, string nombre, int? valor)
        {
            parametros.Add(nombre, valor, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return parametros;
        }

        #endregion Parametros de entrada

        #region Parametros de Salida

        public static T ParametroSalidaValor<T>(this DynamicParameters parametros, string nombre)
        {
            return parametros.Get<T>(nombre);
        }

        public static DynamicParameters ParametroSalidaLong(this DynamicParameters parametros, string nombre)
        {
            parametros.Add(nombre, dbType: DbType.Int64, direction: ParameterDirection.Output);

            return parametros;
        }

        #endregion Parametros de Salida
    }
}