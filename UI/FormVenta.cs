using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentas.UI.Helpers;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Entidades;

namespace SistemaVentas.UI
{
    public class FormVenta : Form
    {
        private DataGridView dgvVentas;
        private Button btnNueva;
        private Button btnVer;
        private Button btnRecargar;
        private Label lblTotal;
        private ComboBox cboEstado;

        public FormVenta()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "Ventas";
            this.Width = 1100;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior con título
            var pnlTitulo = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 50,
                BackColor = System.Drawing.Color.FromArgb(230, 126, 34)
            };

            var lbl = new Label 
            { 
                Text = "Gestión de Ventas", 
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlTitulo.Controls.Add(lbl);
            this.Controls.Add(pnlTitulo);

            // Panel de botones y filtros
            var pnlBotones = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 60,
                Padding = new Padding(10)
            };

            btnNueva = new Button { Text = "? Nueva Venta", Width = 120, Height = 35 };
            btnNueva.Click += (s, e) => MessageBox.Show("Función de nueva venta aún no implementada");

            btnVer = new Button { Text = "?? Ver Detalles", Width = 120, Height = 35, Left = 130 };
            btnVer.Click += (s, e) => MessageBox.Show("Función de ver detalles aún no implementada");

            btnRecargar = new Button { Text = "? Recargar", Width = 100, Height = 35, Left = 260 };
            btnRecargar.Click += (s, e) => CargarDatos();

            // Filtro por estado
            var lblEstado = new Label 
            { 
                Text = "Estado:", 
                Left = 380, 
                Top = 7, 
                AutoSize = true 
            };

            cboEstado = new ComboBox 
            { 
                Left = 430, 
                Top = 5, 
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboEstado.Items.Add("Todos");
            cboEstado.Items.Add("Pendiente");
            cboEstado.Items.Add("Completada");
            cboEstado.Items.Add("Anulada");
            cboEstado.SelectedIndex = 0;
            cboEstado.SelectedIndexChanged += (s, e) => CargarDatos();

            pnlBotones.Controls.Add(btnNueva);
            pnlBotones.Controls.Add(btnVer);
            pnlBotones.Controls.Add(btnRecargar);
            pnlBotones.Controls.Add(lblEstado);
            pnlBotones.Controls.Add(cboEstado);

            this.Controls.Add(pnlBotones);

            // DataGridView
            dgvVentas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = System.Drawing.Color.White
            };

            this.Controls.Add(dgvVentas);

            // Panel inferior con información
            var pnlFooter = new Panel 
            { 
                Dock = DockStyle.Bottom, 
                Height = 30,
                BackColor = System.Drawing.Color.FromArgb(236, 240, 241)
            };

            lblTotal = new Label 
            { 
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            pnlFooter.Controls.Add(lblTotal);
            this.Controls.Add(pnlFooter);

            ThemeHelper.AplicarTemaFormulario(this);
            ThemeHelper.AplicarTemaCompleto(this);
        }

        private void CargarDatos()
        {
            try
            {
                var repo = new VentaRepository();
                List<Venta> ventas = repo.ObtenerTodas();

                dgvVentas.DataSource = ventas;

                // Personalizar columnas
                if (dgvVentas.Columns.Count > 0)
                {
                    dgvVentas.Columns["Id"].Width = 50;
                    dgvVentas.Columns["NumeroVenta"].Width = 100;
                    dgvVentas.Columns["FechaVenta"].Width = 100;
                    dgvVentas.Columns["ClienteId"].Width = 80;
                    dgvVentas.Columns["Total"].Width = 100;
                    dgvVentas.Columns["Estado"].Width = 100;
                    
                    if (dgvVentas.Columns.Contains("Total"))
                    {
                        dgvVentas.Columns["Total"].DefaultCellStyle.Format = "C2";
                    }
                }

                lblTotal.Text = $"Total de ventas: {ventas.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
