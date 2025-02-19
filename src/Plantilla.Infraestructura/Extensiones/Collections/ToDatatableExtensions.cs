using System.Data;
using System.Reflection;

namespace Plantilla.Infraestructura.Extensiones.Collections
{
    public static class ToDatatableExtensions
    {
        public static DataTable ConvertirADataTable<T>(this ICollection<T> lista)
        {
            var table = new DataTable();

            // Obtener todas las propiedades del tipo T
            PropertyInfo[] propiedades = typeof(T).GetProperties();

            // Crear columnas en el DataTable basadas en las propiedades del objeto
            foreach (var propiedad in propiedades)
            {
                table.Columns.Add(propiedad.Name, propiedad.PropertyType);
            }

            // Agregar filas al DataTable con los valores de cada objeto en la lista
            foreach (var item in lista)
            {
                var row = table.NewRow();
                foreach (var propiedad in propiedades)
                {
                    row[propiedad.Name] = propiedad.GetValue(item);
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}