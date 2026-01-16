// ============================================
// Archivo: Extensions/ClienteExtensions.cs
// ============================================

using SistemaVentas.Entidades;

namespace SistemaVentas.Negocio.Extensions
{
    public static class ClienteExtensions
    {
        /// <summary>
        /// Obtiene el total de compras del cliente
        /// </summary>
        public static decimal TotalCompras(this Cliente cliente)
        {
            return cliente.Ventas
                .Where(v => v.Estado == EstadoVenta.Completada)
                .Sum(v => v.Total);
        }

        /// <summary>
        /// Obtiene el promedio de compras del cliente
        /// </summary>
        public static decimal PromedioCompras(this Cliente cliente)
        {
            var ventas = cliente.Ventas
                .Where(v => v.Estado == EstadoVenta.Completada)
                .ToList();

            return ventas.Any() ? ventas.Average(v => v.Total) : 0;
        }

        /// <summary>
        /// Verifica si el cliente es frecuente (más de N compras)
        /// </summary>
        public static bool EsClienteFrecuente(this Cliente cliente, int minimoCompras = 5)
        {
            return cliente.Ventas
                .Count(v => v.Estado == EstadoVenta.Completada) >= minimoCompras;
        }

        /// <summary>
        /// Formatea la información del cliente
        /// </summary>
        public static string FormatearInfo(this Cliente cliente)
        {
            return $"[{cliente.TipoDocumento}:{cliente.NumeroDocumento}] " +
                   $"{cliente.NombreCompleto} - {cliente.Email ?? "Sin email"}";
        }
    }
}
