using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using SistemaVentas.UI.Helpers;

namespace SistemaVentas.UI.UserControls
{
    /// <summary>
    /// User Control reutilizable para funcionalidad de búsqueda
    /// Implementa búsqueda con Enter y botón, placeholder text, y evento personalizado
    /// </summary>
    public partial class UserControlBusqueda : UserControl
    {
        #region Campos Privados

        private string _placeholderText = "Buscar...";
        private Color _placeholderColor = Color.Gray;
        private Color _normalTextColor = Color.Black;
        private bool _isPlaceholder = true;

        #endregion

        #region Eventos Personalizados

        /// <summary>
        /// Evento que se dispara cuando se realiza una búsqueda
        /// </summary>
        public event EventHandler<BusquedaEventArgs> OnBuscar;

        /// <summary>
        /// Evento que se dispara cuando se limpia la búsqueda
        /// </summary>
        public event EventHandler OnLimpiar;

        #endregion

        #region Propiedades Públicas

        /// <summary>
        /// Texto de placeholder que se muestra cuando el campo está vacío
        /// </summary>
        [Category("Appearance")]
        [Description("Texto que se muestra como placeholder cuando el campo está vacío.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                if (_isPlaceholder && !txtBuscar.Focused)
                {
                    MostrarPlaceholder();
                }
            }
        }

        /// <summary>
        /// Obtiene el texto de búsqueda ingresado por el usuario
        /// </summary>
        [Browsable(false)]
        public string TextoBusqueda
        {
            get => _isPlaceholder ? string.Empty : txtBuscar.Text.Trim();
        }

        /// <summary>
        /// Color del texto del placeholder
        /// </summary>
        [Category("Appearance")]
        [Description("Color del texto del placeholder.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set
            {
                _placeholderColor = value;
                if (_isPlaceholder)
                {
                    txtBuscar.ForeColor = _placeholderColor;
                }
            }
        }

        /// <summary>
        /// Habilita o deshabilita el control completo
        /// </summary>
        [Category("Behavior")]
        [Description("Habilita o deshabilita el control de búsqueda.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Habilitado
        {
            get => txtBuscar.Enabled;
            set
            {
                txtBuscar.Enabled = value;
                btnBuscar.Enabled = value;
                btnLimpiar.Enabled = value;
            }
        }

        #endregion

        #region Constructor

        public UserControlBusqueda()
        {
            InitializeComponent();
            ConfigurarControl();
        }

        #endregion

        #region Métodos de Configuración

        private void ConfigurarControl()
        {
            // Configurar TextBox
            txtBuscar.Font = new Font("Segoe UI", 10F);
            MostrarPlaceholder();

            // Configurar botones con tema
            ThemeHelper.AplicarTemaBoton(btnBuscar, true);
            ThemeHelper.AplicarTemaBoton(btnLimpiar, false);

            // Ajustar tamaño de botones
            btnBuscar.Size = new Size(80, txtBuscar.Height);
            btnLimpiar.Size = new Size(80, txtBuscar.Height);

            // Configurar iconos si están disponibles en resources del proyecto (se omite referencia directa)
            try
            {
                // Si existen iconos en recursos del proyecto, asignarlos aquí.
                // Ejemplo (requiere clase Resources auto-generada): btnBuscar.Image = Properties.Resources.search_icon;
            }
            catch
            {
                // Si no existen los iconos, usar solo texto
                btnBuscar.Text = "🔍 Buscar";
                btnLimpiar.Text = "✖ Limpiar";
            }
        }

        private void MostrarPlaceholder()
        {
            _isPlaceholder = true;
            txtBuscar.Text = _placeholderText;
            txtBuscar.ForeColor = _placeholderColor;
            txtBuscar.Font = new Font(txtBuscar.Font, FontStyle.Italic);
        }

        private void OcultarPlaceholder()
        {
            _isPlaceholder = false;
            txtBuscar.Text = string.Empty;
            txtBuscar.ForeColor = _normalTextColor;
            txtBuscar.Font = new Font(txtBuscar.Font, FontStyle.Regular);
        }

        #endregion

        #region Eventos del TextBox

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (_isPlaceholder)
            {
                OcultarPlaceholder();
            }
        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MostrarPlaceholder();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Buscar al presionar Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                RealizarBusqueda();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Habilitar/deshabilitar botón limpiar según haya texto
            btnLimpiar.Enabled = !_isPlaceholder && !string.IsNullOrWhiteSpace(txtBuscar.Text);
        }

        #endregion

        #region Eventos de Botones

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            RealizarBusqueda();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarBusqueda();
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Limpia el texto de búsqueda y vuelve al placeholder
        /// </summary>
        public void LimpiarBusqueda()
        {
            MostrarPlaceholder();

            // Disparar evento de limpiar
            OnLimpiar?.Invoke(this, EventArgs.Empty);

            txtBuscar.Focus();
        }

        /// <summary>
        /// Establece el foco en el campo de búsqueda
        /// </summary>
        public new void Focus()
        {
            txtBuscar.Focus();
        }

        /// <summary>
        /// Establece un texto inicial para buscar
        /// </summary>
        public void EstablecerTexto(string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                OcultarPlaceholder();
                txtBuscar.Text = texto;
            }
            else
            {
                MostrarPlaceholder();
            }
        }

        #endregion

        #region Métodos Privados

        private void RealizarBusqueda()
        {
            if (_isPlaceholder || string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MessageBox.Show(
                    "Por favor ingrese un término de búsqueda",
                    "Búsqueda",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtBuscar.Focus();
                return;
            }

            // Disparar evento con el texto de búsqueda
            var args = new BusquedaEventArgs(txtBuscar.Text.Trim());
            OnBuscar?.Invoke(this, args);
        }

        #endregion
    }

    #region Clase de Argumentos de Evento

    /// <summary>
    /// Argumentos del evento de búsqueda
    /// </summary>
    public class BusquedaEventArgs : EventArgs
    {
        public string TextoBusqueda { get; }
        public DateTime FechaBusqueda { get; }

        public BusquedaEventArgs(string textoBusqueda)
        {
            TextoBusqueda = textoBusqueda;
            FechaBusqueda = DateTime.Now;
        }
    }

    #endregion
}

// ==========================================
// Designer Code
// ==========================================
namespace SistemaVentas.UI.UserControls
{
    partial class UserControlBusqueda
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Controls.Add(this.txtBuscar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBuscar, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLimpiar, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 35);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBuscar.Location = new System.Drawing.Point(3, 3);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(264, 29);
            this.txtBuscar.TabIndex = 0;
            this.txtBuscar.Enter += new System.EventHandler(this.txtBuscar_Enter);
            this.txtBuscar.Leave += new System.EventHandler(this.txtBuscar_Leave);
            this.txtBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBuscar_KeyPress);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBuscar.Location = new System.Drawing.Point(273, 3);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(84, 29);
            this.btnBuscar.TabIndex = 1;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLimpiar.Location = new System.Drawing.Point(363, 3);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(84, 29);
            this.btnLimpiar.TabIndex = 2;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // UserControlBusqueda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserControlBusqueda";
            this.Size = new System.Drawing.Size(450, 35);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}