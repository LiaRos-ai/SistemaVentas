// ============================================
// Archivo: Extensions/VentaExtensions.cs
// ============================================

using SistemaVentas.Entidades;

namespace SistemaVentas.Negocio.Extensions
{
    public static class VentaExtensions
    {
        /// <summary>
        /// Verifica si la venta puede ser anulada
        /// </summary>
        public static bool PuedeAnularse(this Venta venta)
        {
            return venta.Estado == EstadoVenta.Completada
                && (DateTime.Now - venta.FechaVenta).Days <= 7;
        }

        /// <summary>
        /// Obtiene los días transcurridos desde la venta
        /// </summary>
        public static int DiasDesdeVenta(this Venta venta)
        {
            return (DateTime.Now - venta.FechaVenta).Days;
        }

        /// <summary>
        /// Formatea un resumen de la venta
        /// </summary>
        public static string FormatearResumen(this Venta venta)
        {
            return $"Venta #{venta.NumeroVenta} - " +
                   $"Cliente: {venta.Cliente?.NombreCompleto ?? "N/A"} - " +
                   $"Total: ${venta.Total:N2} - " +
                   $"Estado: {venta.Estado}";
        }

        /// <summary>
        /// Calcula la comisión de la venta
        /// </summary>
        public static decimal CalcularComision(this Venta venta, decimal porcentaje = 5m)
        {
            return venta.Total * (porcentaje / 100);
        }

        /// <summary>
        /// Filtra ventas por rango de fechas
        /// </summary>
        public static IEnumerable<Venta> EntreFechas(
            this IEnumerable<Venta> ventas,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return ventas.Where(v =>
                v.FechaVenta.Date >= fechaInicio.Date &&
                v.FechaVenta.Date <= fechaFin.Date);
        }

        /// <summary>
        /// Obtiene el total de ventas en un período
        /// </summary>
        public static decimal TotalEnPeriodo(
            this IEnumerable<Venta> ventas,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return ventas
                .EntreFechas(fechaInicio, fechaFin)
                .Where(v => v.Estado == EstadoVenta.Completada)
                .Sum(v => v.Total);
        }
    }
}