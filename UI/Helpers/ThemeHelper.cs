using System;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaVentas.UI.Helpers
{
    /// <summary>
    /// Clase helper para aplicar temas visuales consistentes en toda la aplicación
    /// Implementa paleta de colores y estilos predefinidos
    /// </summary>
    public static class ThemeHelper
    {
        #region Métodos para Botones

        /// <summary>
        /// Aplica el tema visual a un botón
        /// </summary>
        /// <param name="btn">Botón a estilizar</param>
        /// <param name="esPrimario">True para color primario, False para acento</param>
        public static void AplicarTemaBoton(Button btn, bool esPrimario = true)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); // Transparente
            btn.BackColor = esPrimario ? AppColors.Primary : AppColors.Accent;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.Padding = new Padding(10, 5, 10, 5);
            btn.MinimumSize = new Size(100, 35);

            // Efectos hover
            Color colorOriginal = btn.BackColor;
            Color colorHover = esPrimario ? AppColors.PrimaryDark : DarkenColor(AppColors.Accent, 0.2f);

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = colorHover;
                btn.FlatAppearance.MouseOverBackColor = colorHover;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = colorOriginal;
            };
        }

        /// <summary>
        /// Aplica estilo de botón de acción crítica (eliminar, cancelar)
        /// </summary>
        public static void AplicarTemaBotonPeligro(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = AppColors.Error;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.Padding = new Padding(10, 5, 10, 5);

            Color colorHover = DarkenColor(AppColors.Error, 0.2f);
            btn.MouseEnter += (s, e) => btn.BackColor = colorHover;
            btn.MouseLeave += (s, e) => btn.BackColor = AppColors.Error;
        }

        /// <summary>
        /// Aplica estilo de botón de éxito (guardar, confirmar)
        /// </summary>
        public static void AplicarTemaBotonExito(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = AppColors.Success;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.Padding = new Padding(10, 5, 10, 5);

            Color colorHover = DarkenColor(AppColors.Success, 0.2f);
            btn.MouseEnter += (s, e) => btn.BackColor = colorHover;
            btn.MouseLeave += (s, e) => btn.BackColor = AppColors.Success;
        }

        /// <summary>
        /// Aplica estilo de botón secundario (contorno)
        /// </summary>
        public static void AplicarTemaBotonSecundario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = AppColors.Primary;
            btn.BackColor = Color.White;
            btn.ForeColor = AppColors.Primary;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.Padding = new Padding(10, 5, 10, 5);

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = AppColors.PrimaryLight;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.White;
            };
        }

        #endregion

        #region Métodos para DataGridView

        /// <summary>
        /// Aplica tema visual completo a un DataGridView
        /// </summary>
        public static void AplicarTemaDataGridView(DataGridView dgv)
        {
            // Configuración general
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = AppColors.Primary;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.BackgroundColor = Color.White;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.EnableHeadersVisualStyles = false;

            // Estilo de encabezados
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = AppColors.Primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgv.ColumnHeadersHeight = 40;

            // Estilo de celdas
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.RowTemplate.Height = 35;
        }

        #endregion

        #region Métodos para Paneles y Formularios

        /// <summary>
        /// Aplica estilo de panel de título/cabecera
        /// </summary>
        public static void AplicarTemaPanelTitulo(Panel panel)
        {
            panel.BackColor = AppColors.Primary;
            panel.Padding = new Padding(15);
            panel.Dock = DockStyle.Top;
            panel.Height = 60;

            // Si el panel contiene un Label, estilizarlo también
            foreach (Control control in panel.Controls)
            {
                if (control is Label lbl)
                {
                    lbl.ForeColor = Color.White;
                    lbl.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                }
            }
        }

        /// <summary>
        /// Aplica tema a formularios secundarios
        /// </summary>
        public static void AplicarTemaFormulario(Form form)
        {
            form.BackColor = AppColors.Background;
            form.Font = new Font("Segoe UI", 9F);
            form.StartPosition = FormStartPosition.CenterParent;
        }

        #endregion

        #region Métodos para TextBox y ComboBox

        /// <summary>
        /// Aplica estilo moderno a un TextBox
        /// </summary>
        public static void AplicarTemaTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 10F);
            txt.Padding = new Padding(5);
            txt.Height = 30;
        }

        /// <summary>
        /// Aplica estilo moderno a un ComboBox
        /// </summary>
        public static void AplicarTemaComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = new Font("Segoe UI", 10F);
            cmb.Height = 30;
            cmb.BackColor = Color.White;
        }

        #endregion

        #region Métodos para Labels

        /// <summary>
        /// Aplica estilo de título principal
        /// </summary>
        public static void AplicarEstiloTitulo(System.Windows.Forms.Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lbl.ForeColor = AppColors.TextPrimary;
        }

        /// <summary>
        /// Aplica estilo de subtítulo
        /// </summary>
        public static void AplicarEstiloSubtitulo(System.Windows.Forms.Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.ForeColor = AppColors.TextPrimary;
        }

        /// <summary>
        /// Aplica estilo de etiqueta de campo
        /// </summary>
        public static void AplicarEstiloEtiqueta(System.Windows.Forms.Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.ForeColor = AppColors.TextPrimary;
        }

        #endregion

        #region Métodos para GroupBox

        /// <summary>
        /// Aplica tema a un GroupBox
        /// </summary>
        public static void AplicarTemaGroupBox(GroupBox grp)
        {
            grp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grp.ForeColor = AppColors.Primary;
            grp.Padding = new Padding(10);
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Oscurece un color en un porcentaje dado
        /// </summary>
        private static Color DarkenColor(Color color, float percent)
        {
            int r = (int)(color.R * (1 - percent));
            int g = (int)(color.G * (1 - percent));
            int b = (int)(color.B * (1 - percent));
            return Color.FromArgb(color.A, r, g, b);
        }

        /// <summary>
        /// Aclara un color en un porcentaje dado
        /// </summary>
        private static Color LightenColor(Color color, float percent)
        {
            int r = color.R + (int)((255 - color.R) * percent);
            int g = color.G + (int)((255 - color.G) * percent);
            int b = color.B + (int)((255 - color.B) * percent);
            return Color.FromArgb(color.A, r, g, b);
        }

        #endregion

        #region Aplicar Tema Completo a Formulario

        /// <summary>
        /// Aplica el tema completo recursivamente a todos los controles de un formulario
        /// </summary>
        public static void AplicarTemaCompleto(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                // Aplicar tema según tipo de control
                if (control is Button btn)
                {
                    if (btn.Name.Contains("Guardar") || btn.Name.Contains("Aceptar"))
                        AplicarTemaBotonExito(btn);
                    else if (btn.Name.Contains("Eliminar") || btn.Name.Contains("Cancelar"))
                        AplicarTemaBotonPeligro(btn);
                    else
                        AplicarTemaBoton(btn);
                }
                else if (control is DataGridView dgv)
                {
                    AplicarTemaDataGridView(dgv);
                }
                else if (control is Panel panel)
                {
                    if (panel.Name.Contains("Titulo") || panel.Name.Contains("Header"))
                        AplicarTemaPanelTitulo(panel);
                }
                else if (control is TextBox txt)
                {
                    AplicarTemaTextBox(txt);
                }
                else if (control is ComboBox cmb)
                {
                    AplicarTemaComboBox(cmb);
                }
                else if (control is GroupBox grp)
                {
                    AplicarTemaGroupBox(grp);
                }
                else if (control is System.Windows.Forms.Label lbl)
                {
                    if (lbl.Name.Contains("Titulo"))
                        AplicarEstiloTitulo(lbl);
                    else if (lbl.Name.Contains("Subtitulo"))
                        AplicarEstiloSubtitulo(lbl);
                    else
                        AplicarEstiloEtiqueta(lbl);
                }

                // Aplicar recursivamente a contenedores
                if (control.HasChildren)
                {
                    AplicarTemaCompleto(control);
                }
            }
        }

        #endregion
    }
}