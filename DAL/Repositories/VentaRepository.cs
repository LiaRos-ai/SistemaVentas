// ============================================
// Archivo: Repositories/VentaRepository.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaVentas.Entidades;

namespace SistemaVentas.DAL.Repositories
{
    public class VentaRepository : BaseRepository
    {
        public List<Venta> ObtenerTodas()
        {
            List<Venta> ventas = new List<Venta>();

            string query = @"
                SELECT v.Id, v.NumeroVenta, v.ClienteId, v.UsuarioId, v.FechaVenta,
                       v.SubTotal, v.Impuesto, v.Total, v.Estado
                FROM Ventas v
                ORDER BY v.FechaVenta DESC";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ventas.Add(MapearVenta(reader));
                    }
                }
            }

            return ventas;
        }

        public Venta ObtenerPorId(int id)
        {
            Venta venta = null;

            string query = @"
                SELECT v.Id, v.NumeroVenta, v.ClienteId, v.UsuarioId, v.FechaVenta,
                       v.SubTotal, v.Impuesto, v.Total, v.Estado
                FROM Ventas v
                WHERE v.Id = @Id";

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
                            venta = MapearVenta(reader);
                        }
                    }
                }
            }

            return venta;
        }

        public int Insertar(Venta venta)
        {
            string query = @"
                INSERT INTO Ventas (NumeroVenta, ClienteId, UsuarioId, FechaVenta,
                                   SubTotal, Impuesto, Total, Estado)
                VALUES (@NumeroVenta, @ClienteId, @UsuarioId, @FechaVenta,
                       @SubTotal, @Impuesto, @Total, @Estado);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NumeroVenta", venta.NumeroVenta ?? "");
                    cmd.Parameters.AddWithValue("@ClienteId", venta.ClienteId);
                    cmd.Parameters.AddWithValue("@UsuarioId", venta.UsuarioId);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@SubTotal", venta.SubTotal);
                    cmd.Parameters.AddWithValue("@Impuesto", venta.Impuesto);
                    cmd.Parameters.AddWithValue("@Total", venta.Total);
                    cmd.Parameters.AddWithValue("@Estado", venta.Estado.ToString());

                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
        }

        public bool Actualizar(Venta venta)
        {
            string query = @"
                UPDATE Ventas
                SET NumeroVenta = @NumeroVenta,
                    ClienteId = @ClienteId,
                    UsuarioId = @UsuarioId,
                    FechaVenta = @FechaVenta,
                    SubTotal = @SubTotal,
                    Impuesto = @Impuesto,
                    Total = @Total,
                    Estado = @Estado
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", venta.Id);
                    cmd.Parameters.AddWithValue("@NumeroVenta", venta.NumeroVenta ?? "");
                    cmd.Parameters.AddWithValue("@ClienteId", venta.ClienteId);
                    cmd.Parameters.AddWithValue("@UsuarioId", venta.UsuarioId);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@SubTotal", venta.SubTotal);
                    cmd.Parameters.AddWithValue("@Impuesto", venta.Impuesto);
                    cmd.Parameters.AddWithValue("@Total", venta.Total);
                    cmd.Parameters.AddWithValue("@Estado", venta.Estado.ToString());

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            string query = @"
                DELETE FROM Ventas
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

        public List<Venta> ObtenerPorEstado(EstadoVenta estado)
        {
            List<Venta> ventas = new List<Venta>();

            string query = @"
                SELECT v.Id, v.NumeroVenta, v.ClienteId, v.UsuarioId, v.FechaVenta,
                       v.SubTotal, v.Impuesto, v.Total, v.Estado
                FROM Ventas v
                WHERE v.Estado = @Estado
                ORDER BY v.FechaVenta DESC";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Estado", estado.ToString());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(MapearVenta(reader));
                        }
                    }
                }
            }

            return ventas;
        }

        private Venta MapearVenta(SqlDataReader reader)
        {
            return new Venta
            {
                Id = reader.GetInt32(0),
                NumeroVenta = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                ClienteId = reader.GetInt32(2),
                UsuarioId = reader.GetInt32(3),
                FechaVenta = reader.GetDateTime(4),
                SubTotal = reader.GetDecimal(5),
                Impuesto = reader.GetDecimal(6),
                Total = reader.GetDecimal(7),
                Estado = (EstadoVenta)Enum.Parse(typeof(EstadoVenta), reader.GetString(8))
            };
        }
    }
}
