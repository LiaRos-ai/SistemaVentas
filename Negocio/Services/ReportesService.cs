using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentas.DAL.DataSets;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Entidades;

namespace SistemaVentas.Negocio.Services
{
    public class ReportesService
    {
        private readonly ProductoRepository _productoRepo;

        // Constructor por defecto: crea su propio repositorio
        public ReportesService() : this(new ProductoRepository()) { }

        // Permite inyectar un repositorio (útil para pruebas)
        public ReportesService(ProductoRepository productoRepo)
        {
            _productoRepo = productoRepo;
        }

        public Task<DataTable> ObtenerProductosParaReporte()
        {
            DataTable dt = ProductosReporteDS.CrearEstructura();

            var productos = _productoRepo.ObtenerTodos()
                .Where(p => p.Activo)
                .OrderBy(p => p.CategoriaNombre)
                .ThenBy(p => p.Nombre)
                .ToList();

            foreach (var producto in productos)
            {
                DataRow row = dt.NewRow();
                row["ProductoId"] = producto.Id;
                row["Codigo"] = producto.Codigo ?? string.Empty;
                row["Nombre"] = producto.Nombre ?? string.Empty;
                row["Categoria"] = producto.CategoriaNombre ?? producto.Categoria?.Nombre ?? "Sin categoría";
                row["Precio"] = producto.PrecioVenta;
                row["Stock"] = producto.Stock;
                row["Estado"] = producto.Stock < 10 ? "BAJO STOCK" :
                               producto.Stock < 50 ? "NORMAL" : "ALTO STOCK";

                dt.Rows.Add(row);
            }

            return Task.FromResult(dt);
        }
    }
}
