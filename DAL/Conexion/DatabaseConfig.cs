// ============================================
// CAPA DAL - Configuración de Base de Datos
// Archivo: DatabaseConfig.cs
// ============================================

using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SistemaVentas.DAL.Conexion
{
    /// <summary>
    /// Gestiona la configuración y conexión a la base de datos
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Obtiene la cadena de conexión desde App.config
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                // Intentar primero la cadena específica del proyecto/ejecutable (SistemaVentas),
                return ConfigurationManager.ConnectionStrings["SistemaVentas"]?.ConnectionString
                    //?? ConfigurationManager.ConnectionStrings["SistemaVentasDB"]?.ConnectionString
                    ?? "Data Source=localhost;Initial Catalog=SistemaVentas;Integrated Security=True;TrustServerCertificate=True;";
            }
        }

        /// <summary>
        /// Crea y retorna una nueva conexión a la base de datos
        /// </summary>
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Prueba la conexión a la base de datos
        /// </summary>
        /// <returns>True si la conexión es exitosa</returns>
        public static bool ProbarConexion()
        {
            try
            {
                using (var conexion = ObtenerConexion())
                {
                    conexion.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene información detallada de la conexión y diagnostica problemas
        /// </summary>
        public static string ObtenerDiagnosticoConexion()
        {
            var diagnostico = new System.Text.StringBuilder();
            diagnostico.AppendLine("═══════════════════════════════════════════════");
            diagnostico.AppendLine("  DIAGNÓSTICO DE CONEXIÓN A BASE DE DATOS");
            diagnostico.AppendLine("═══════════════════════════════════════════════");
            diagnostico.AppendLine();

            // 1. Mostrar cadena de conexión (sin contraseña)
            var connString = ConnectionString;
            var connStringSegura = OcultarContraseña(connString);
            diagnostico.AppendLine($"📋 Cadena de conexión:");
            diagnostico.AppendLine($"   {connStringSegura}");
            diagnostico.AppendLine();

            // 2. Verificar si App.config está siendo leído
            try
            {
                var ventasConfig = ConfigurationManager.ConnectionStrings["SistemaVentas"];
                //var sistemaVentasConfig = ConfigurationManager.ConnectionStrings["SistemaVentasDB"];

                diagnostico.AppendLine("🔍 Cadenas de conexión detectadas en App.config:");
                diagnostico.AppendLine($"   • 'SistemaVentas': {(ventasConfig != null ? "✓ Encontrada" : "✗ No encontrada")}");
                //diagnostico.AppendLine($"   • 'SistemaVentasDB': {(sistemaVentasConfig != null ? "✓ Encontrada" : "✗ No encontrada")}");
                diagnostico.AppendLine();
            }
            catch (Exception ex)
            {
                diagnostico.AppendLine($"⚠️  Error al leer App.config: {ex.Message}");
                diagnostico.AppendLine();
            }

            // 3. Intentar conectar
            try
            {
                using (var conexion = ObtenerConexion())
                {
                    conexion.Open();
                    diagnostico.AppendLine("✓ CONEXIÓN EXITOSA");
                    diagnostico.AppendLine($"   Servidor: {conexion.DataSource}");
                    diagnostico.AppendLine($"   Base de datos: {conexion.Database}");
                    diagnostico.AppendLine($"   Estado: Abierta");
                    diagnostico.AppendLine();
                }
            }
            catch (SqlException sqlEx)
            {
                diagnostico.AppendLine("✗ ERROR DE CONEXIÓN SQL");
                diagnostico.AppendLine($"   Código de error: {sqlEx.Number}");
                diagnostico.AppendLine($"   Mensaje: {sqlEx.Message}");
                diagnostico.AppendLine();

                // Ayuda contextual según el error
                diagnostico.AppendLine("💡 Sugerencias:");
                switch (sqlEx.Number)
                {
                    case 18456:
                        diagnostico.AppendLine("   • Error de autenticación (usuario/contraseña)");
                        diagnostico.AppendLine("   • Verifica el 'User ID' y 'Password' en la cadena de conexión");
                        break;
                    case 53:
                    case -1:
                        diagnostico.AppendLine("   • No se puede conectar al servidor (red/firewall)");
                        diagnostico.AppendLine("   • Verifica que SQL Server está corriendo");
                        diagnostico.AppendLine("   • Verifica el nombre/IP del servidor");
                        break;
                    case 4060:
                        diagnostico.AppendLine("   • La base de datos no existe o no hay permisos");
                        diagnostico.AppendLine("   • Verifica que 'Ventas' existe en SQL Server");
                        break;
                    default:
                        diagnostico.AppendLine($"   • Error desconocido ({sqlEx.Number})");
                        diagnostico.AppendLine("   • Busca este código en documentación de SQL Server");
                        break;
                }
                diagnostico.AppendLine();
            }
            catch (Exception ex)
            {
                diagnostico.AppendLine($"✗ ERROR GENERAL: {ex.GetType().Name}");
                diagnostico.AppendLine($"   Mensaje: {ex.Message}");
                diagnostico.AppendLine();
            }

            diagnostico.AppendLine("═══════════════════════════════════════════════");

            return diagnostico.ToString();
        }

        /// <summary>
        /// Obtiene información del servidor y base de datos
        /// </summary>
        public static string ObtenerInfoConexion()
        {
            try
            {
                using (var conexion = ObtenerConexion())
                {
                    conexion.Open();
                    return $"Servidor: {conexion.DataSource} | Base de datos: {conexion.Database}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Oculta la contraseña en la cadena de conexión para mostrar de forma segura
        /// </summary>
        private static string OcultarContraseña(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "[cadena vacía]";

            return System.Text.RegularExpressions.Regex.Replace(
                connectionString,
                @"(Password\s*=\s*)([^;]+)",
                "$1****",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}