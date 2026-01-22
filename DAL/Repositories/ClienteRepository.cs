// ============================================
// Archivo: Repositories/ClienteRepository.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaVentas.Entidades;

namespace SistemaVentas.DAL.Repositories
{
    public class ClienteRepository : BaseRepository
    {
        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> clientes = new List<Cliente>();

            string query = @"
                SELECT Id, TipoDocumento, NumeroDocumento, Nombres, Apellidos,
                       Direccion, Telefono, Email, Activo, FechaRegistro
                FROM Clientes
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
                        clientes.Add(MapearCliente(reader));
                    }
                }
            }

            return clientes;
        }

        public Cliente ObtenerPorId(int id)
        {
            Cliente cliente = null;

            string query = @"
                SELECT Id, TipoDocumento, NumeroDocumento, Nombres, Apellidos,
                       Direccion, Telefono, Email, Activo, FechaRegistro
                FROM Clientes
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
                            cliente = MapearCliente(reader);
                        }
                    }
                }
            }

            return cliente;
        }

        public int Insertar(Cliente cliente)
        {
            string query = @"
                INSERT INTO Clientes (TipoDocumento, NumeroDocumento, Nombres, Apellidos,
                                     Direccion, Telefono, Email, Activo, FechaRegistro)
                VALUES (@TipoDocumento, @NumeroDocumento, @Nombres, @Apellidos,
                       @Direccion, @Telefono, @Email, 1, GETDATE());
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TipoDocumento", cliente.TipoDocumento.ToString());
                    cmd.Parameters.AddWithValue("@NumeroDocumento", cliente.NumeroDocumento ?? "");
                    cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres ?? "");
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos ?? "");
                    cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? "");
                    cmd.Parameters.AddWithValue("@Email", cliente.Email ?? "");

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
        }

        public bool Actualizar(Cliente cliente)
        {
            string query = @"
                UPDATE Clientes
                SET TipoDocumento = @TipoDocumento,
                    NumeroDocumento = @NumeroDocumento,
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    Direccion = @Direccion,
                    Telefono = @Telefono,
                    Email = @Email
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.Parameters.AddWithValue("@TipoDocumento", cliente.TipoDocumento.ToString());
                    cmd.Parameters.AddWithValue("@NumeroDocumento", cliente.NumeroDocumento ?? "");
                    cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres ?? "");
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos ?? "");
                    cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? "");
                    cmd.Parameters.AddWithValue("@Email", cliente.Email ?? "");

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            string query = @"
                UPDATE Clientes
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

        public List<Cliente> BuscarPorNombre(string nombre)
        {
            List<Cliente> clientes = new List<Cliente>();

            string query = @"
                SELECT Id, TipoDocumento, NumeroDocumento, Nombres, Apellidos,
                       Direccion, Telefono, Email, Activo, FechaRegistro
                FROM Clientes
                WHERE (Nombres LIKE @Nombre OR Apellidos LIKE @Nombre) 
                  AND Activo = 1
                ORDER BY Nombres";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", $"%{nombre}%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(MapearCliente(reader));
                        }
                    }
                }
            }

            return clientes;
        }

        private Cliente MapearCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                Id = reader.GetInt32(0),
                TipoDocumento = (TipoDocumento)Enum.Parse(typeof(TipoDocumento), reader.GetString(1)),
                NumeroDocumento = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Nombres = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Apellidos = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Direccion = reader.IsDBNull(5) ? null : reader.GetString(5),
                Telefono = reader.IsDBNull(6) ? null : reader.GetString(6),
                Email = reader.IsDBNull(7) ? null : reader.GetString(7),
                Activo = reader.GetBoolean(8),
                FechaRegistro = reader.GetDateTime(9)
            };
        }
    }
}
