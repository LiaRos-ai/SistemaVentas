// ============================================
// Archivo: Categoria.cs
// ============================================

namespace SistemaVentas.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public virtual ICollection<Producto> Productos { get; set; }
            = new List<Producto>();
    }
}
