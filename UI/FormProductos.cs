using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentas.UI.Helpers;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Entidades;

namespace SistemaVentas.UI
{
    public class FormProductos : Form
    {
        private DataGridView dgvProductos;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnRecargar;
        private Label lblTotal;
        private TextBox txtBuscar;

        public FormProductos()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "Productos";
            this.Width = 1000;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior con título
            var pnlTitulo = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 50,
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219)
            };

            var lbl = new Label 
            { 
                Text = "Mantenimiento de Productos", 
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlTitulo.Controls.Add(lbl);
            this.Controls.Add(pnlTitulo);

            // Panel de botones y búsqueda
            var pnlBotones = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 60,
                Padding = new Padding(10)
            };

            btnNuevo = new Button { Text = "? Nuevo", Width = 100, Height = 35 };
            btnNuevo.Click += (s, e) => MessageBox.Show("Función de nuevo aún no implementada");

            btnEditar = new Button { Text = "? Editar", Width = 100, Height = 35, Left = 110 };
            btnEditar.Click += (s, e) => MessageBox.Show("Función de editar aún no implementada");

            btnEliminar = new Button { Text = "? Eliminar", Width = 100, Height = 35, Left = 220 };
            btnEliminar.Click += (s, e) => MessageBox.Show("Función de eliminar aún no implementada");

            btnRecargar = new Button { Text = "? Recargar", Width = 100, Height = 35, Left = 330 };
            btnRecargar.Click += (s, e) => CargarDatos();

            // Búsqueda
            var lblBuscar = new Label 
            { 
                Text = "Buscar:", 
                Left = 450, 
                Top = 7, 
                AutoSize = true 
            };

            txtBuscar = new TextBox 
            { 
                Left = 510, 
                Top = 5, 
                Width = 200, 
                Height = 25 
            };
            txtBuscar.KeyPress += (s, e) => 
            {
                if (e.KeyChar == (char)Keys.Return)
                {
                    e.Handled = true;
                    BuscarProductos();
                }
            };

            pnlBotones.Controls.Add(btnNuevo);
            pnlBotones.Controls.Add(btnEditar);
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnRecargar);
            pnlBotones.Controls.Add(lblBuscar);
            pnlBotones.Controls.Add(txtBuscar);

            this.Controls.Add(pnlBotones);

            // DataGridView
            dgvProductos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = System.Drawing.Color.White
            };

            this.Controls.Add(dgvProductos);

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
                var repo = new ProductoRepository();
                List<Producto> productos = repo.ObtenerTodos();

                dgvProductos.DataSource = productos;

                // Personalizar columnas
                if (dgvProductos.Columns.Count > 0)
                {
                    dgvProductos.Columns["Id"].Width = 50;
                    dgvProductos.Columns["Codigo"].Width = 100;
                    dgvProductos.Columns["Nombre"].Width = 200;
                    dgvProductos.Columns["PrecioVenta"].Width = 100;
                    dgvProductos.Columns["Stock"].Width = 80;
                    dgvProductos.Columns["PrecioVenta"].DefaultCellStyle.Format = "C2";
                }

                lblTotal.Text = $"Total de productos: {productos.Count}";
                txtBuscar.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarProductos()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarDatos();
                    return;
                }

                var repo = new ProductoRepository();
                List<Producto> productos = repo.BuscarPorNombre(txtBuscar.Text);

                dgvProductos.DataSource = productos;
                lblTotal.Text = $"Productos encontrados: {productos.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
