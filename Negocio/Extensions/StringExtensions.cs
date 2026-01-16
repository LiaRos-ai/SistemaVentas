// ============================================
// Archivo: Extensions/StringExtensions.cs
// ============================================

namespace SistemaVentas.Negocio.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Verifica si el string es numérico
        /// </summary>
        public static bool EsNumerico(this string texto)
        {
            return double.TryParse(texto, out _);
        }

        /// <summary>
        /// Trunca el texto a una longitud específica
        /// </summary>
        public static string Truncar(this string texto, int longitud)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length <= longitud)
                return texto;

            return texto.Substring(0, longitud) + "...";
        }

        /// <summary>
        /// Valida el formato de email
        /// </summary>
        public static bool EsEmailValido(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Capitaliza la primera letra de cada palabra
        /// </summary>
        public static string CapitalizarPalabras(this string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(texto.ToLower());
        }
    }
}