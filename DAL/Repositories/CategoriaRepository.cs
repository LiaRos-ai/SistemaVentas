// ============================================
// Archivo: Repositories/CategoriaRepository.cs
// ============================================

using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaVentas.Entidades;

namespace SistemaVentas.DAL.Repositories
{
    // Repositorio mínimo para listar categorías (suficiente para compilar y pruebas básicas)
    public class CategoriaRepository : BaseRepository
    {
        public List<Categoria> ObtenerTodas()
        {
            var categorias = new List<Categoria>();

            string query = "SELECT Id, Nombre, Descripcion, Activo FROM Categorias WHERE Activo = 1 ORDER BY Nombre";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categorias.Add(new Categoria
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        });
                    }
                }
            }

            return categorias;
        }
    }
}
