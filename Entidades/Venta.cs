// ============================================
// Archivo: Venta.cs
// ============================================

//using SistemaVentas.Entidades.Enums;

namespace SistemaVentas.Entidades
{
    public class Venta
    {
        public int Id { get; set; }

        public string NumeroVenta { get; set; } = string.Empty;

        public int ClienteId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime FechaVenta { get; set; } = DateTime.Now;

        public decimal SubTotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public EstadoVenta Estado { get; set; } = EstadoVenta.Pendiente;

        // Navegación
        public virtual Cliente? Cliente { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
            = new List<DetalleVenta>();

        /// <summary>
        /// Calcula los totales de la venta
        /// </summary>
        public void CalcularTotales()
        {
            SubTotal = DetallesVenta.Sum(d => d.SubTotal);
            Impuesto = SubTotal * 0.13m; // 13% IVA
            Total = SubTotal + Impuesto;
        }

        /// <summary>
        /// Genera el número de venta automáticamente
        /// </summary>
        public void GenerarNumeroVenta()
        {
            NumeroVenta = $"V-{DateTime.Now:yyyyMMdd}-{Id:D6}";
        }

        /// <summary>
        /// Verifica si la venta puede ser anulada
        /// </summary>
        public bool PuedeAnularse()
        {
            return Estado == EstadoVenta.Completada
                && (DateTime.Now - FechaVenta).Days <= 7;
        }
    }
}
