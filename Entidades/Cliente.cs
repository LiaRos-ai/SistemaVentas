// ============================================
// SISTEMA DE VENTAS - CAPA DE ENTIDADES
// Archivo: Cliente.cs
// ============================================

//using SistemaVentas.Entidades.Enums;
using System;
using System.Collections.Generic;

namespace SistemaVentas.Entidades
{
    /// <summary>
    /// Representa un cliente del sistema de ventas
    /// </summary>
    public class Cliente
    {
        public int Id { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedad calculada
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        // Navegación
        public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        /// <summary>
        /// Valida el formato del documento según el tipo
        /// </summary>
        public bool ValidarDocumento()
        {
            return TipoDocumento switch
            {
                TipoDocumento.DNI => NumeroDocumento.Length == 8,
                TipoDocumento.RUC => NumeroDocumento.Length == 11,
                TipoDocumento.Carnet => NumeroDocumento.Length >= 7,
                _ => false
            };
        }
    }
}
