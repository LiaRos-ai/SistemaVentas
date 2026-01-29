using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentas.UI.Helpers;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Entidades;
using SistemaVentas.DAL.Conexion;

namespace SistemaVentas.UI
{
    public class FormClientes : Form
    {
        private DataGridView dgvClientes;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnRecargar;
        private Label lblTotal;

        public FormClientes()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "Clientes";
            this.Width = 1000;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior con t?tulo
            var pnlTitulo = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 50,
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113)
            };

            var lbl = new Label 
            { 
                Text = "Mantenimiento de Clientes", 
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlTitulo.Controls.Add(lbl);
            this.Controls.Add(pnlTitulo);

            // Panel de botones
            var pnlBotones = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 50,
                Padding = new Padding(10)
            };

            btnNuevo = new Button { Text = "? Nuevo", Width = 100, Height = 35 };
            btnNuevo.Click += (s, e) => MessageBox.Show("Funci?n de nuevo a?n no implementada");

            btnEditar = new Button { Text = "? Editar", Width = 100, Height = 35, Left = 110 };
            btnEditar.Click += (s, e) => MessageBox.Show("Funci?n de editar a?n no implementada");

            btnEliminar = new Button { Text = "? Eliminar", Width = 100, Height = 35, Left = 220 };
            btnEliminar.Click += (s, e) => MessageBox.Show("Funci?n de eliminar a?n no implementada");

            btnRecargar = new Button { Text = "? Recargar", Width = 100, Height = 35, Left = 330 };
            btnRecargar.Click += (s, e) => CargarDatos();

            pnlBotones.Controls.Add(btnNuevo);
            pnlBotones.Controls.Add(btnEditar);
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnRecargar);

            this.Controls.Add(pnlBotones);

            // DataGridView
            dgvClientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = System.Drawing.Color.White
            };

            this.Controls.Add(dgvClientes);

            // Panel inferior con informaci?n
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
                var repo = new ClienteRepository();
                List<Cliente> clientes = repo.ObtenerTodos();

                // Limpiar DataSource antes de asignar
                dgvClientes.DataSource = null;
                dgvClientes.Rows.Clear();

                dgvClientes.DataSource = clientes;

                // Personalizar columnas
                if (dgvClientes.Columns.Count > 0)
                {
                    if (dgvClientes.Columns.Contains("Id")) dgvClientes.Columns["Id"].Width = 50;
                    if (dgvClientes.Columns.Contains("Nombres")) dgvClientes.Columns["Nombres"].Width = 150;
                    if (dgvClientes.Columns.Contains("Apellidos")) dgvClientes.Columns["Apellidos"].Width = 120;
                    if (dgvClientes.Columns.Contains("NumeroDocumento")) dgvClientes.Columns["NumeroDocumento"].Width = 120;
                    if (dgvClientes.Columns.Contains("Telefono")) dgvClientes.Columns["Telefono"].Width = 100;
                    if (dgvClientes.Columns.Contains("Email")) dgvClientes.Columns["Email"].Width = 150;
                }

                lblTotal.Text = $"Total de clientes: {clientes.Count}";

                if (clientes.Count == 0)
                {
                    // Mostrar diagnóstico de conexión o sugerencias
                    string diag = DatabaseConfig.ObtenerDiagnosticoConexion();
                    MessageBox.Show("No se encontraron clientes. Revisar conexión y base de datos.\n\n" + diag,
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

