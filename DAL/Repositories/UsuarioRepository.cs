// ============================================
// Archivo: Repositories/UsuarioRepository.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaVentas.Entidades;

namespace SistemaVentas.DAL.Repositories
{
    public class UsuarioRepository : BaseRepository
    {
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = @"
                SELECT Id, NombreUsuario, Clave, Nombres, Apellidos, 
                       Email, EsAdministrador, Activo, FechaRegistro
                FROM Usuarios
                WHERE Activo = 1
                ORDER BY Nombres";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(MapearUsuario(reader));
                    }
                }
            }

            return usuarios;
        }

        public Usuario ObtenerPorId(int id)
        {
            Usuario usuario = null;

            string query = @"
                SELECT Id, NombreUsuario, Clave, Nombres, Apellidos, 
                       Email, EsAdministrador, Activo, FechaRegistro
                FROM Usuarios
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = MapearUsuario(reader);
                        }
                    }
                }
            }

            return usuario;
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            Usuario usuario = null;

            string query = @"
                SELECT Id, NombreUsuario, Clave, Nombres, Apellidos, 
                       Email, EsAdministrador, Activo, FechaRegistro
                FROM Usuarios
                WHERE NombreUsuario = @NombreUsuario";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = MapearUsuario(reader);
                        }
                    }
                }
            }

            return usuario;
        }

        public int Insertar(Usuario usuario)
        {
            string query = @"
                INSERT INTO Usuarios (NombreUsuario, Clave, Nombres, Apellidos,
                                     Email, EsAdministrador, Activo, FechaRegistro)
                VALUES (@NombreUsuario, @Clave, @Nombres, @Apellidos,
                       @Email, @EsAdministrador, 1, GETDATE());
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario ?? "");
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave ?? "");
                    cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres ?? "");
                    cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos ?? "");
                    cmd.Parameters.AddWithValue("@Email", usuario.Email ?? "");
                    cmd.Parameters.AddWithValue("@EsAdministrador", usuario.EsAdministrador);

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
        }

        public bool Actualizar(Usuario usuario)
        {
            string query = @"
                UPDATE Usuarios
                SET NombreUsuario = @NombreUsuario,
                    Clave = @Clave,
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    Email = @Email,
                    EsAdministrador = @EsAdministrador
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", usuario.Id);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario ?? "");
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave ?? "");
                    cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres ?? "");
                    cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos ?? "");
                    cmd.Parameters.AddWithValue("@Email", usuario.Email ?? "");
                    cmd.Parameters.AddWithValue("@EsAdministrador", usuario.EsAdministrador);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            string query = @"
                UPDATE Usuarios
                SET Activo = 0
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        private Usuario MapearUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                Id = reader.GetInt32(0),
                NombreUsuario = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Clave = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Nombres = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Apellidos = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                EsAdministrador = reader.GetBoolean(6),
                Activo = reader.GetBoolean(7),
                FechaRegistro = reader.GetDateTime(8)
            };
        }
    }
}
