// ============================================
// Archivo: Producto.cs
// ============================================

namespace SistemaVentas.Entidades
{
    /// <summary>
    /// Representa un producto del catálogo
    /// </summary>
    public class Producto
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int CategoriaId { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public int StockMinimo { get; set; } = 5;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedades calculadas
        public decimal Ganancia => PrecioVenta - PrecioCompra;

        public bool StockBajo => Stock <= StockMinimo;

        // Navegación
        public virtual Categoria? Categoria { get; set; }

        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
            = new List<DetalleVenta>();

        /// <summary>
        /// Actualiza el stock del producto
        /// </summary>
        public void ActualizarStock(int cantidad)
        {
            if (Stock + cantidad < 0)
                throw new InvalidOperationException("Stock insuficiente");

            Stock += cantidad;
        }

        /// <summary>
        /// Calcula el subtotal para una cantidad determinada
        /// </summary>
        public decimal CalcularSubtotal(int cantidad)
        {
            return PrecioVenta * cantidad;
        }
    }
}
