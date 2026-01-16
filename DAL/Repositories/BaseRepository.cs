// ============================================
// Archivo: Repositories/BaseRepository.cs
// ============================================

using System;
using System.Data.SqlClient;
using SistemaVentas.DAL.Conexion;

namespace SistemaVentas.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }

        protected BaseRepository()
        {
            ConnectionString = DatabaseConfig.ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected void EjecutarComando(string query,
            Action<SqlCommand> configurarParametros = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    configurarParametros?.Invoke(cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected T EjecutarEscalar<T>(string query,
            Action<SqlCommand> configurarParametros = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    configurarParametros?.Invoke(cmd);
                    object resultado = cmd.ExecuteScalar();
                    return (T)Convert.ChangeType(resultado, typeof(T));
                }
            }
        }
    }
}
