using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentas.UI.Helpers;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Entidades;

namespace SistemaVentas.UI
{
    public class FormCategorias : Form
    {
        private DataGridView dgvCategorias;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnRecargar;
        private Label lblTotal;

        public FormCategorias()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "Categorías";
            this.Width = 900;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior con título
            var pnlTitulo = new Panel 
            { 
                Dock = DockStyle.Top, 
                Height = 50,
                BackColor = System.Drawing.Color.FromArgb(41, 128, 185)
            };

            var lbl = new Label 
            { 
                Text = "Mantenimiento de Categorías", 
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
            btnNuevo.Click += (s, e) => MessageBox.Show("Función de nuevo aún no implementada");

            btnEditar = new Button { Text = "? Editar", Width = 100, Height = 35, Left = 110 };
            btnEditar.Click += (s, e) => MessageBox.Show("Función de editar aún no implementada");

            btnEliminar = new Button { Text = "? Eliminar", Width = 100, Height = 35, Left = 220 };
            btnEliminar.Click += (s, e) => MessageBox.Show("Función de eliminar aún no implementada");

            btnRecargar = new Button { Text = "? Recargar", Width = 100, Height = 35, Left = 330 };
            btnRecargar.Click += (s, e) => CargarDatos();

            pnlBotones.Controls.Add(btnNuevo);
            pnlBotones.Controls.Add(btnEditar);
            pnlBotones.Controls.Add(btnEliminar);
            pnlBotones.Controls.Add(btnRecargar);

            this.Controls.Add(pnlBotones);

            // DataGridView
            dgvCategorias = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = System.Drawing.Color.White
            };

            this.Controls.Add(dgvCategorias);

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
                var repo = new CategoriaRepository();
                List<Categoria> categorias = repo.ObtenerTodas();

                dgvCategorias.DataSource = categorias;

                // Personalizar columnas
                if (dgvCategorias.Columns.Count > 0)
                {
                    // Desactivar AutoSizeColumnsMode para poder establecer anchos personalizados
                    dgvCategorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    
                    dgvCategorias.Columns["Id"].Width = 50;
                    dgvCategorias.Columns["Nombre"].Width = 200;
                    dgvCategorias.Columns["Descripcion"].Width = 400;
                }

                lblTotal.Text = $"Total de categorías: {categorias.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar categorías: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
