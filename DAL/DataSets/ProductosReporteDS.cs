using System.Data;

namespace SistemaVentas.DAL.DataSets
{
    public class ProductosReporteDS
    {
        public static DataTable CrearEstructura()
        {
            DataTable dt = new DataTable("Productos");

            dt.Columns.Add("ProductoId", typeof(int));
            dt.Columns.Add("Codigo", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Categoria", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Estado", typeof(string));

            return dt;
        }
    }
}
