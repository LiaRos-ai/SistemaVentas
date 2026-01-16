// ============================================
// Archivo: DetalleVenta.cs
// ============================================

namespace SistemaVentas.Entidades
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        // Navegación
        public virtual Venta? Venta { get; set; }

        public virtual Producto? Producto { get; set; }

        /// <summary>
        /// Calcula el subtotal del detalle
        /// </summary>
        public void CalcularSubTotal()
        {
            SubTotal = Cantidad * PrecioUnitario;
        }
    }
}
