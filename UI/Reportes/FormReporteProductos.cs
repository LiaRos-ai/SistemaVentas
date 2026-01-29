using System;
using System.Windows.Forms;
using SistemaVentas.Negocio.Services;
using SistemaVentas.UI.Helpers;
using System.Linq;

namespace SistemaVentas.UI.Reportes
{
    /// <summary>
    /// Formulario para mostrar el reporte de productos
    /// </summary>
    public partial class FormReporteProductos : Form
    {
        private ReportesService _reportesService;

        public FormReporteProductos()
        {
            InitializeComponent();
            this.Text = "Reporte de Productos";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Aplicar tema
            ThemeHelper.AplicarTemaFormulario(this);

            // Inicializar servicio de reportes
            _reportesService = new ReportesService();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarReporte();
        }

        private async void CargarReporte()
        {
            try
            {
                // Mostrar mensaje de carga
                Cursor = Cursors.WaitCursor;

                // Obtener datos del reporte desde el servicio
                var dtProductos = await _reportesService.ObtenerProductosParaReporte();

                if (dtProductos == null || dtProductos.Rows.Count == 0)
                {
                    MessageBox.Show("No hay productos para mostrar en el reporte.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Encontrar el DataGridView en los controles
                var controls = this.Controls.Find("dgvReporte", true);
                if (controls.Length > 0 && controls[0] is DataGridView dgv)
                {
                    // Vincular datos al DataGridView
                    dgv.DataSource = dtProductos;

                    // Configurar ancho de columnas
                    foreach (System.Windows.Forms.DataGridViewColumn col in dgv.Columns)
                    {
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    // Formatear columna de precio
                    if (dgv.Columns.Contains("Precio") && dgv.Columns["Precio"] != null)
                    {
                        dgv.Columns["Precio"]!.DefaultCellStyle.Format = "C2";
                        dgv.Columns["Precio"]!.DefaultCellStyle.Alignment = 
                            DataGridViewContentAlignment.MiddleRight;
                    }

                    // Hacer el DataGridView de solo lectura
                    dgv.ReadOnly = true;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }

                // Actualizar etiqueta de estadísticas
                var lblControls = this.Controls.Find("lblEstadisticas", true);
                if (lblControls.Length > 0 && lblControls[0] is Label lbl)
                    lbl.Text = $"Total de productos: {dtProductos.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    saveFileDialog.FileName = $"ReporteProductos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var controls = this.Controls.Find("dgvReporte", true);
                        if (controls.Length > 0 && controls[0] is DataGridView dgv)
                        {
                            ExportarACSV(dgv, saveFileDialog.FileName);
                            MessageBox.Show("Archivo exportado correctamente.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarACSV(DataGridView dgv, string rutaArchivo)
        {
            using (System.IO.StreamWriter writer = System.IO.File.CreateText(rutaArchivo))
            {
                // Escribir encabezados
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    writer.Write($"\"{dgv.Columns[i].HeaderText}\"");
                    if (i < dgv.Columns.Count - 1)
                        writer.Write(",");
                }
                writer.WriteLine();

                // Escribir filas
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        var value = row.Cells[i].Value?.ToString() ?? "";
                        writer.Write($"\"{value}\"");
                        if (i < dgv.Columns.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }

        private void InitializeComponent()
        {
            // Panel superior con botones
            Panel panelBotones = new Panel();
            panelBotones.Dock = DockStyle.Top;
            panelBotones.Height = 50;
            panelBotones.Padding = new Padding(10);

            Button btnExportar = new Button();
            btnExportar.Text = "Exportar a CSV";
            btnExportar.Size = new System.Drawing.Size(120, 30);
            btnExportar.Location = new System.Drawing.Point(10, 10);
            btnExportar.Click += (object? s, EventArgs e) => BtnExportar_Click(s, e);
            panelBotones.Controls.Add(btnExportar);

            Button btnActualizar = new Button();
            btnActualizar.Text = "Actualizar";
            btnActualizar.Size = new System.Drawing.Size(100, 30);
            btnActualizar.Location = new System.Drawing.Point(140, 10);
            btnActualizar.Click += (s, e) => CargarReporte();
            panelBotones.Controls.Add(btnActualizar);

            // DataGridView
            DataGridView dgvReporte = new DataGridView();
            dgvReporte.Dock = DockStyle.Fill;
            dgvReporte.Name = "dgvReporte";
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Panel inferior con etiqueta de estadísticas
            Panel panelEstadisticas = new Panel();
            panelEstadisticas.Dock = DockStyle.Bottom;
            panelEstadisticas.Height = 30;
            panelEstadisticas.BorderStyle = BorderStyle.FixedSingle;

            Label lblEstadisticas = new Label();
            lblEstadisticas.Name = "lblEstadisticas";
            lblEstadisticas.Dock = DockStyle.Fill;
            lblEstadisticas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblEstadisticas.Padding = new Padding(10, 0, 0, 0);
            lblEstadisticas.Text = "Cargando datos...";
            panelEstadisticas.Controls.Add(lblEstadisticas);

            this.SuspendLayout();
            this.Controls.Add(dgvReporte);
            this.Controls.Add(panelEstadisticas);
            this.Controls.Add(panelBotones);

            this.Name = "FormReporteProductos";
            this.ResumeLayout(false);
        }
    }
}
