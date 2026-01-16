// ============================================
// SISTEMA DE VENTAS - EXTENSION METHODS
// Archivo: Extensions/ProductoExtensions.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentas.Entidades;

namespace SistemaVentas.Negocio.Extensions
{
    /// <summary>
    /// Métodos de extensión para la entidad Producto
    /// </summary>
    public static class ProductoExtensions
    {
        /// <summary>
        /// Verifica si hay stock suficiente para la cantidad solicitada
        /// </summary>
        public static bool TieneStockSuficiente(this Producto producto, int cantidad)
        {
            return producto.Stock >= cantidad;
        }

        /// <summary>
        /// Verifica si el producto requiere reabastecimiento
        /// </summary>
        public static bool RequiereReabastecimiento(this Producto producto)
        {
            return producto.Stock <= producto.StockMinimo;
        }

        /// <summary>
        /// Calcula la ganancia potencial del stock actual
        /// </summary>
        public static decimal GananciaPotencial(this Producto producto)
        {
            return producto.Ganancia * producto.Stock;
        }

        /// <summary>
        /// Aplica un descuento al precio del producto
        /// </summary>
        public static decimal AplicarDescuento(this Producto producto, decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El descuento debe estar entre 0 y 100");

            return producto.PrecioVenta * (1 - porcentajeDescuento / 100);
        }

        /// <summary>
        /// Formatea la información del producto para mostrar
        /// </summary>
        public static string FormatearInfo(this Producto producto)
        {
            var estado = producto.RequiereReabastecimiento()
                ? "⚠ STOCK BAJO"
                : "✓ Disponible";

            return $"[{producto.Codigo}] {producto.Nombre} - " +
                   $"${producto.PrecioVenta:N2} - Stock: {producto.Stock} {estado}";
        }

        /// <summary>
        /// Calcula la ganancia total de una colección de productos
        /// </summary>
        public static decimal CalcularGananciaTotal(this IEnumerable<Producto> productos)
        {
            return productos.Sum(p => p.GananciaPotencial());
        }
    }
}