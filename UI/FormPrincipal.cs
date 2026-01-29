using SistemaVentas.Entidades;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaVentas.UI.Helpers;

namespace SistemaVentas.UI
{
    /// <summary>
    /// Formulario principal MDI del Sistema de Ventas
    /// Implementa navegación principal, menús y organización de ventanas
    /// </summary>
    public partial class FormPrincipal : Form
    {
        #region Constructor

        public FormPrincipal()
        {
            InitializeComponent();
            // Aplicar tema al formulario principal y a sus controles
            ThemeHelper.AplicarTemaFormulario(this);
            ThemeHelper.AplicarTemaCompleto(this);
            ConfigurarFormulario();
            ConfigurarStatusBar();
        }

        #endregion

        #region Configuración Inicial

        private System.Windows.Forms.Timer _statusTimer;

        private void ConfigurarFormulario()
        {
            // Configurar como contenedor MDI
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Sistema de Ventas - v1.0";
            this.BackColor = AppColors.Background;

            // Configurar icono (si existe) - evitar referencia a Properties.Resources si no existe
            try
            {
                // Si tiene recursos embebidos, puede cargarse aquí. Se omite referencia directa a Properties.
            }
            catch { /* Si no existe el icono, continuar sin él */ }
        }

        private void ConfigurarStatusBar()
        {
            // Actualizar información en status bar
            UpdateStatusBarUsuario();
            tssFecha.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            tssHora.Text = DateTime.Now.ToString("HH:mm:ss");

            // Timer para actualizar hora
            _statusTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _statusTimer.Tick += (s, e) => tssHora.Text = DateTime.Now.ToString("HH:mm:ss");
            _statusTimer.Start();

            // Suscribirse a cambios de sesión para actualizar usuario en status bar
            SesionActual.OnSesionCambiada += UpdateStatusBarUsuario;
        }

        private void UpdateStatusBarUsuario()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatusBarUsuario));
                return;
            }

            tssUsuario.Text = $"Usuario: {SesionActual.UsuarioActual?.NombreCompleto ?? "Invitado"}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                _statusTimer?.Stop();
                SesionActual.OnSesionCambiada -= UpdateStatusBarUsuario;
            }
            catch { }
        }

        #endregion

        #region Eventos del Menú - Archivo

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Determinar qué formulario abrir según contexto
            MessageBox.Show("Función Nuevo", "Información",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarAplicacion();
        }

        private void CerrarAplicacion()
        {
            var resultado = MessageBox.Show(
                "¿Está seguro que desea salir del sistema?",
                "Confirmar Salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Cerrar todas las ventanas hijas
                foreach (Form hijo in this.MdiChildren)
                {
                    hijo.Close();
                }

                Application.Exit();
            }
        }

        #endregion

        #region Eventos del Menú - Mantenimiento

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormProductos", "Mantenimiento de Productos");
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormCategorias", "Mantenimiento de Categorías");
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormClientes", "Mantenimiento de Clientes");
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormUsuarios", "Mantenimiento de Usuarios");
        }

        #endregion

        #region Eventos del Menú - Transacciones

        private void nuevaVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormVenta", "Nueva Venta", crearNueva: true);
        }

        private void consultarVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorNombre("FormConsultaVentas", "Consultar Ventas");
        }

        #endregion

        #region Eventos del Menú - Reportes

        private void ventasPorPeriodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mostrar el reporte de productos
            AbrirFormularioReporte();
        }

        private void productosConStockBajoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reporte de Productos con Stock Bajo\n(Por implementar)",
                "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirFormularioReporte()
        {
            try
            {
                // Verificar si ya existe una instancia abierta del formulario
                Form? formularioExistente = this.MdiChildren.FirstOrDefault(f => f.GetType().Name == "FormReporteProductos");
                
                if (formularioExistente != null)
                {
                    formularioExistente.Activate();
                    return;
                }

                // Crear e instanciar el formulario de reporte
                var formReporte = new SistemaVentas.UI.Reportes.FormReporteProductos();
                formReporte.MdiParent = this;
                formReporte.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Eventos del Menú - Ventana

        private void cascadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mosaicaHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mosaicaVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void cerrarTodasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form hijo in this.MdiChildren)
            {
                hijo.Close();
            }
        }

        #endregion

        #region Eventos del Menú - Ayuda

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Intentar abrir formulario AcercaDe si existe, sino mostrar cuadro simple
            if (!AbrirFormularioPorNombre("FormAcercaDe", "Acerca de..."))
            {
                MessageBox.Show(
                    "Sistema de Ventas\nVersión 1.0\nDesarrollado con .NET",
                    "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Eventos de la Barra de Herramientas

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            nuevaVentaToolStripMenuItem_Click(sender, e);
        }

        private void tsbProductos_Click(object sender, EventArgs e)
        {
            productosToolStripMenuItem_Click(sender, e);
        }

        private void tsbClientes_Click(object sender, EventArgs e)
        {
            clientesToolStripMenuItem_Click(sender, e);
        }

        private void tsbReportes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Reportes\n(Por implementar)",
                "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            CerrarAplicacion();
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Abre un formulario hijo MDI por nombre de tipo (busca el tipo en los assemblies cargados)
        /// </summary>
        private bool AbrirFormularioPorNombre(string tipoSimpleName, string titulo, bool crearNueva = false)
        {
            // Buscar tipo por nombre simple o por nombre completo dentro de los assemblies cargados
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type? tipo = null;

            foreach (var asm in assemblies)
            {
                tipo = asm.GetTypes().FirstOrDefault(t => t.Name.Equals(tipoSimpleName, StringComparison.OrdinalIgnoreCase)
                    || t.FullName?.EndsWith($".{tipoSimpleName}") == true);
                if (tipo != null) break;
            }

            if (tipo == null)
            {
                MessageBox.Show($"El formulario '{tipoSimpleName}' no está disponible en esta compilación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Verificar si es un Form
            if (!typeof(Form).IsAssignableFrom(tipo))
            {
                MessageBox.Show($"El tipo '{tipoSimpleName}' encontrado no es un formulario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Si no se requiere nueva instancia, validar si ya está abierta
            if (!crearNueva)
            {
                foreach (Form hijo in this.MdiChildren)
                {
                    if (hijo.GetType() == tipo)
                    {
                        hijo.Activate();
                        return true;
                    }
                }
            }

            try
            {
                var instancia = (Form?)Activator.CreateInstance(tipo!);
                if (instancia == null)
                {
                    MessageBox.Show($"No se pudo crear la instancia de '{tipoSimpleName}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Aplicar tema automáticamente al formulario hijo antes de mostrarlo
                try
                {
                    ThemeHelper.AplicarTemaFormulario(instancia);
                    ThemeHelper.AplicarTemaCompleto(instancia);
                }
                catch { /* no bloquear si el tema falla */ }

                instancia.MdiParent = this;
                instancia.Text = titulo;
                instancia.Show();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Clase de Sesión

        /// <summary>
        /// Clase estática para mantener información de sesión actual
        /// </summary>
        public static class SesionActual
        {
            public static Usuario? UsuarioActual { get; private set; }
            public static DateTime FechaLogin { get; private set; } = DateTime.MinValue;

            // Evento para notificar cambios de sesión
            public static event Action? OnSesionCambiada;

            public static bool EsAdministrador()
            {
                return UsuarioActual?.EsAdministrador ?? false;
            }

            public static void IniciarSesion(Usuario usuario)
            {
                UsuarioActual = usuario;
                FechaLogin = DateTime.Now;
                OnSesionCambiada?.Invoke();
            }

            public static void CerrarSesion()
            {
                UsuarioActual = null;
                FechaLogin = DateTime.MinValue;
                OnSesionCambiada?.Invoke();
            }
        }

        #endregion
    }

    #region Clase de Colores del Sistema

    /// <summary>
    /// Paleta de colores del sistema para consistencia visual
    /// </summary>
    public static class AppColors
    {
        // Colores primarios
        public static Color Primary = ColorTranslator.FromHtml("#2196F3");
        public static Color PrimaryDark = ColorTranslator.FromHtml("#1976D2");
        public static Color PrimaryLight = ColorTranslator.FromHtml("#BBDEFB");

        // Colores de acento
        public static Color Accent = ColorTranslator.FromHtml("#FF9800");

        // Colores de estado
        public static Color Success = ColorTranslator.FromHtml("#4CAF50");
        public static Color Warning = ColorTranslator.FromHtml("#FFC107");
        public static Color Error = ColorTranslator.FromHtml("#F44336");
        public static Color Info = ColorTranslator.FromHtml("#2196F3");

        // Colores de texto
        public static Color TextPrimary = ColorTranslator.FromHtml("#212121");
        public static Color TextSecondary = ColorTranslator.FromHtml("#757575");

        // Backgrounds
        public static Color Background = ColorTranslator.FromHtml("#FAFAFA");
        public static Color Surface = Color.White;
    }

    #endregion
}