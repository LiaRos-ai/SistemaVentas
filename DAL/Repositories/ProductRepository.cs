// ============================================
// Archivo: Repositories/ProductoRepository.cs
// ============================================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaVentas.Entidades;

namespace SistemaVentas.DAL.Repositories
{
    public class ProductoRepository : BaseRepository
    {
        public List<Producto> ObtenerTodos()
        {
            List<Producto> productos = new List<Producto>();

            string query = @"
                SELECT p.Id, p.Codigo, p.Nombre, p.Descripcion,
                       p.PrecioCompra, p.PrecioVenta, p.Stock, 
                       p.StockMinimo, p.CategoriaId, c.Nombre as CategoriaNombre, 
                       p.Activo
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.Id
                WHERE p.Activo = 1
                ORDER BY p.Nombre";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(MapearProducto(reader));
                    }
                }
            }

            return productos;
        }

        public Producto ObtenerPorId(int id)
        {
            Producto producto = null;

            string query = @"
                SELECT p.Id, p.Codigo, p.Nombre, p.Descripcion,
                       p.PrecioCompra, p.PrecioVenta, p.Stock, 
                       p.StockMinimo, p.CategoriaId, c.Nombre as CategoriaNombre, 
                       p.Activo
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.Id
                WHERE p.Id = @Id";

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
                            producto = MapearProducto(reader);
                        }
                    }
                }
            }

            return producto;
        }

        public int Insertar(Producto producto)
        {
            string query = @"
                INSERT INTO Productos 
                (Codigo, Nombre, Descripcion, PrecioCompra, PrecioVenta, 
                 Stock, StockMinimo, CategoriaId, Activo, FechaRegistro)
                VALUES 
                (@Codigo, @Nombre, @Descripcion, @PrecioCompra, @PrecioVenta,
                 @Stock, @StockMinimo, @CategoriaId, @Activo, @FechaRegistro);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    ConfigurarParametrosProducto(cmd, producto);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                    int nuevoId = (int)cmd.ExecuteScalar();
                    return nuevoId;
                }
            }
        }

        public bool Actualizar(Producto producto)
        {
            string query = @"
                UPDATE Productos SET
                    Codigo = @Codigo,
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    PrecioCompra = @PrecioCompra,
                    PrecioVenta = @PrecioVenta,
                    Stock = @Stock,
                    StockMinimo = @StockMinimo,
                    CategoriaId = @CategoriaId
                WHERE Id = @Id";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", producto.Id);
                    ConfigurarParametrosProducto(cmd, producto);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public bool Eliminar(int id)
        {
            string query = "UPDATE Productos SET Activo = 0 WHERE Id = @Id";

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

        public List<Producto> BuscarPorNombre(string nombre)
        {
            List<Producto> productos = new List<Producto>();

            string query = @"
                SELECT p.Id, p.Codigo, p.Nombre, p.Descripcion,
                       p.PrecioCompra, p.PrecioVenta, p.Stock, 
                       p.StockMinimo, p.CategoriaId, c.Nombre as CategoriaNombre, 
                       p.Activo
                FROM Productos p
                LEFT JOIN Categorias c ON p.CategoriaId = c.Id
                WHERE p.Activo = 1 AND p.Nombre LIKE @Nombre
                ORDER BY p.Nombre";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(MapearProducto(reader));
                        }
                    }
                }
            }

            return productos;
        }

        public bool ActualizarStock(int productoId, int cantidad)
        {
            string query = @"
                UPDATE Productos 
                SET Stock = Stock + @Cantidad 
                WHERE Id = @ProductoId";

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductoId", productoId);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            var producto = new Producto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Descripcion")),
                PrecioCompra = reader.GetDecimal(reader.GetOrdinal("PrecioCompra")),
                PrecioVenta = reader.GetDecimal(reader.GetOrdinal("PrecioVenta")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                StockMinimo = reader.GetInt32(reader.GetOrdinal("StockMinimo")),
                CategoriaId = reader.GetInt32(reader.GetOrdinal("CategoriaId")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };

            // Si la columna CategoriaNombre existe y no es null, asignarla (prop opcional en entidad)
            try
            {
                int idx = reader.GetOrdinal("CategoriaNombre");
                if (!reader.IsDBNull(idx))
                {
                    var nombreCat = reader.GetString(idx);
                    // Intentar asignar una propiedad opcional en Producto llamada CategoriaNombre si existe
                    var prop = typeof(Producto).GetProperty("CategoriaNombre");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(producto, nombreCat);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // columna no presente - ignorar
            }

            return producto;
        }

        private void ConfigurarParametrosProducto(SqlCommand cmd, Producto producto)
        {
            cmd.Parameters.AddWithValue("@Codigo", producto.Codigo);
            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion",
                producto.Descripcion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);
            cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
            cmd.Parameters.AddWithValue("@Stock", producto.Stock);
            cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);
            cmd.Parameters.AddWithValue("@CategoriaId", producto.CategoriaId);
            cmd.Parameters.AddWithValue("@Activo", producto.Activo);
        }
    }
}