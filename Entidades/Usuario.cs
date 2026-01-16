// ============================================
// Archivo: Usuario.cs
// ============================================

namespace SistemaVentas.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Clave { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string? Email { get; set; }

        public bool EsAdministrador { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Propiedad calculada
        public string NombreCompleto => $"{Nombres} {Apellidos}";

        // Navegación
        public virtual ICollection<Venta> Ventas { get; set; }
            = new List<Venta>();
    }
}
