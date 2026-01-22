// ============================================
// SISTEMA DE VENTAS - PROGRAMA DE PRUEBA
// Archivo: Program.cs (Proyecto ConsoleTest)
// ============================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentas.Entidades;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.DAL.Conexion;
using SistemaVentas.Negocio.Extensions;
using SistemaVentas.UI;

namespace SistemaVentas.ConsoleTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Intentar mostrar la UI sin verificar conexión a BD
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Mostrar menú de demostración
                MostrarMenuUI();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir interfaz gráfica: {ex.Message}");
            }

            // Si falla la UI, continuar con el programa original de consola
            Console.Title = "Sistema de Ventas - Demo";

            // Verificar conexión antes de comenzar
            if (!DatabaseConfig.ProbarConexion())
            {
                Console.WriteLine("❌ ERROR: No se pudo conectar a la base de datos.");
                Console.WriteLine("Verifique la cadena de conexión en App.config");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("✓ Conexión a base de datos exitosa\n");

            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine();

                Console.Clear();

                switch (opcion)
                {
                    case "1":
                        ListarProductos();
                        break;
                    case "2":
                        InsertarProducto();
                        break;
                    case "3":
                        BuscarProducto();
                        break;
                    case "4":
                        ActualizarProducto();
                        break;
                    case "5":
                        EliminarProducto();
                        break;
                    case "6":
                        DemostrarExtensionMethods();
                        break;
                    case "7":
                        ListarCategorias();
                        break;
                    case "8":
                        ProductosBajoStock();
                        break;
                    case "9":
                        ProbarConexion();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\n¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("\n❌ Opción inválida");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\n\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA DE VENTAS - DEMO DÍA 10            ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  CRUD DE PRODUCTOS:");
            Console.WriteLine("  1. Listar todos los productos");
            Console.WriteLine("  2. Insertar nuevo producto");
            Console.WriteLine("  3. Buscar producto por nombre");
            Console.WriteLine("  4. Actualizar producto");
            Console.WriteLine("  5. Eliminar producto (lógico)");
            Console.WriteLine();
            Console.WriteLine("  FUNCIONALIDADES ADICIONALES:");
            Console.WriteLine("  6. Demostrar Extension Methods");
            Console.WriteLine("  7. Listar categorías");
            Console.WriteLine("  8. Productos bajo stock");
            Console.WriteLine("  9. Probar conexión a BD");
            Console.WriteLine();
            Console.WriteLine("  0. Salir");
            Console.WriteLine();
            Console.WriteLine("─────────────────────────────────────────────────");
        }

        static void ListarProductos()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  LISTADO DE PRODUCTOS");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                var repo = new ProductoRepository();
                var productos = repo.ObtenerTodos();

                if (productos.Count == 0)
                {
                    Console.WriteLine("No hay productos registrados.");
                    return;
                }

                Console.WriteLine($"Total de productos: {productos.Count}\n");
                Console.WriteLine("{0,-5} {1,-10} {2,-30} {3,12} {4,8}",
                    "ID", "Código", "Nombre", "Precio", "Stock");
                Console.WriteLine(new string('─', 70));

                foreach (var producto in productos)
                {
                    Console.WriteLine("{0,-5} {1,-10} {2,-30} {3,12:C2} {4,8}",
                        producto.Id,
                        producto.Codigo,
                        producto.Nombre.Truncar(28),
                        producto.PrecioVenta,
                        producto.Stock);
                }

                Console.WriteLine(new string('─', 70));
                Console.WriteLine($"\n✓ {productos.Count} productos listados exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al listar productos: {ex.Message}");
            }
        }

        static void InsertarProducto()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  INSERTAR NUEVO PRODUCTO");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                var producto = new Producto();

                // Solicitar datos
                Console.Write("Código: ");
                producto.Codigo = Console.ReadLine();

                Console.Write("Nombre: ");
                producto.Nombre = Console.ReadLine();

                Console.Write("Descripción: ");
                producto.Descripcion = Console.ReadLine();

                // Mostrar categorías disponibles
                var categoriaRepo = new CategoriaRepository();
                var categorias = categoriaRepo.ObtenerTodas();

                Console.WriteLine("\nCategorías disponibles:");
                foreach (var cat in categorias)
                {
                    Console.WriteLine($"  {cat.Id}. {cat.Nombre}");
                }

                Console.Write("\nID de Categoría: ");
                producto.CategoriaId = int.Parse(Console.ReadLine());

                Console.Write("Precio de Compra: ");
                producto.PrecioCompra = decimal.Parse(Console.ReadLine());

                Console.Write("Precio de Venta: ");
                producto.PrecioVenta = decimal.Parse(Console.ReadLine());

                Console.Write("Stock inicial: ");
                producto.Stock = int.Parse(Console.ReadLine());

                Console.Write("Stock mínimo: ");
                producto.StockMinimo = int.Parse(Console.ReadLine());

                // Insertar en BD
                var repo = new ProductoRepository();
                int nuevoId = repo.Insertar(producto);

                Console.WriteLine($"\n✓ Producto insertado exitosamente con ID: {nuevoId}");
                Console.WriteLine($"  Ganancia por unidad: ${producto.Ganancia:N2}");
            }
            catch (FormatException)
            {
                Console.WriteLine("❌ Error: Formato de dato incorrecto");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al insertar producto: {ex.Message}");
            }
        }

        static void BuscarProducto()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  BUSCAR PRODUCTO");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                Console.Write("Ingrese nombre a buscar: ");
                string nombre = Console.ReadLine();

                var repo = new ProductoRepository();
                var productos = repo.BuscarPorNombre(nombre);

                if (productos.Count == 0)
                {
                    Console.WriteLine($"\n❌ No se encontraron productos con '{nombre}'");
                    return;
                }

                Console.WriteLine($"\n✓ Se encontraron {productos.Count} producto(s):\n");

                foreach (var producto in productos)
                {
                    Console.WriteLine(producto.FormatearInfo());
                    Console.WriteLine($"   Ganancia: ${producto.Ganancia:N2}");
                    Console.WriteLine($"   Ganancia potencial: ${producto.GananciaPotencial():N2}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al buscar: {ex.Message}");
            }
        }

        static void ActualizarProducto()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  ACTUALIZAR PRODUCTO");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                Console.Write("ID del producto a actualizar: ");
                int id = int.Parse(Console.ReadLine());

                var repo = new ProductoRepository();
                var producto = repo.ObtenerPorId(id);

                if (producto == null)
                {
                    Console.WriteLine($"\n❌ No se encontró producto con ID {id}");
                    return;
                }

                Console.WriteLine("\nProducto encontrado:");
                Console.WriteLine(producto.FormatearInfo());
                Console.WriteLine("\nIngrese nuevos datos (Enter para mantener actual):\n");

                Console.Write($"Nombre [{producto.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    producto.Nombre = nombre;

                Console.Write($"Precio Venta [{producto.PrecioVenta}]: ");
                string precio = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(precio))
                    producto.PrecioVenta = decimal.Parse(precio);

                Console.Write($"Stock [{producto.Stock}]: ");
                string stock = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(stock))
                    producto.Stock = int.Parse(stock);

                bool actualizado = repo.Actualizar(producto);

                if (actualizado)
                    Console.WriteLine("\n✓ Producto actualizado exitosamente");
                else
                    Console.WriteLine("\n❌ No se pudo actualizar el producto");
            }
            catch (FormatException)
            {
                Console.WriteLine("❌ Error: Formato de dato incorrecto");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al actualizar: {ex.Message}");
            }
        }

        static void EliminarProducto()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  ELIMINAR PRODUCTO");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                Console.Write("ID del producto a eliminar: ");
                int id = int.Parse(Console.ReadLine());

                var repo = new ProductoRepository();
                var producto = repo.ObtenerPorId(id);

                if (producto == null)
                {
                    Console.WriteLine($"\n❌ No se encontró producto con ID {id}");
                    return;
                }

                Console.WriteLine("\nProducto a eliminar:");
                Console.WriteLine(producto.FormatearInfo());

                Console.Write("\n¿Está seguro? (S/N): ");
                string confirmacion = Console.ReadLine();

                if (confirmacion.ToUpper() == "S")
                {
                    bool eliminado = repo.Eliminar(id);

                    if (eliminado)
                        Console.WriteLine("\n✓ Producto eliminado exitosamente");
                    else
                        Console.WriteLine("\n❌ No se pudo eliminar el producto");
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al eliminar: {ex.Message}");
            }
        }

        static void DemostrarExtensionMethods()
        {
            Console.WriteLine("═══════════════════════════════════════════════");
            Console.WriteLine("  DEMOSTRACIÓN DE EXTENSION METHODS");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            try
            {
                var repo = new ProductoRepository();
                var productos = repo.ObtenerTodos();

                if (!productos.Any())
                {
                    Console.WriteLine("No hay productos para demostrar");
                    return;
                }

                var producto = productos[0];

                Console.WriteLine("Extension Methods disponibles:\n");

                // 1. TieneStockSuficiente
                Console.WriteLine($"1. TieneStockSuficiente(5):");
                Console.WriteLine($"   Producto: {producto.Nombre}");
                Console.WriteLine($"   Stock actual: {producto.Stock}");
                Console.WriteLine($"   ¿Tiene stock suficiente para 5? {producto.TieneStockSuficiente(5)}");

                // 2. RequiereReabastecimiento
                Console.WriteLine($"\n2. RequiereReabastecimiento():");
                Console.WriteLine($"   Stock: {producto.Stock} | Mínimo: {producto.StockMinimo}");
                Console.WriteLine($"   Requiere reabastecimiento: {producto.RequiereReabastecimiento()}");

                // 3. GananciaPotencial
                Console.WriteLine($"\n3. GananciaPotencial():");
                Console.WriteLine($"   Ganancia unitaria: ${producto.Ganancia:N2}");
                Console.WriteLine($"   Stock: {producto.Stock}");
                Console.WriteLine($"   Ganancia potencial: ${producto.GananciaPotencial():N2}");

                // 4. AplicarDescuento
                Console.WriteLine($"\n4. AplicarDescuento(10%):");
                Console.WriteLine($"   Precio normal: ${producto.PrecioVenta:N2}");
                Console.WriteLine($"   Precio con descuento: ${producto.AplicarDescuento(10):N2}");

                // 5. FormatearInfo
                Console.WriteLine($"\n5. FormatearInfo():");
                Console.WriteLine($"   {producto.FormatearInfo()}");

                // Extension methods de colecciones
                Console.WriteLine($"\n6. CalcularGananciaTotal() [colección]:");
                var gananciaTotal = productos.CalcularGananciaTotal();
                Console.WriteLine($"   Ganancia potencial total: ${gananciaTotal:N2}");

                Console.WriteLine("\n✓ Demostración completada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        static void ListarCategorias()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  LISTADO DE CATEGORÍAS");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                var repo = new CategoriaRepository();
                var categorias = repo.ObtenerTodas();

                if (categorias.Count == 0)
                {
                    Console.WriteLine("No hay categorías registradas.");
                    return;
                }

                Console.WriteLine($"Total de categorías: {categorias.Count}\n");
                Console.WriteLine("{0,-5} {1,-20} {2,-40}",
                    "ID", "Nombre", "Descripción");
                Console.WriteLine(new string('─', 70));

                foreach (var categoria in categorias)
                {
                    Console.WriteLine("{0,-5} {1,-20} {2,-40}",
                        categoria.Id,
                        categoria.Nombre,
                        categoria.Descripcion ?? "Sin descripción");
                }

                Console.WriteLine(new string('─', 70));
                Console.WriteLine($"\n✓ {categorias.Count} categorías listadas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        static void ProductosBajoStock()
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("  PRODUCTOS BAJO STOCK");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                var repo = new ProductoRepository();
                var productos = repo.ObtenerTodos();

                var productosBajos = productos
                    .Where(p => p.RequiereReabastecimiento())
                    .ToList();

                if (!productosBajos.Any())
                {
                    Console.WriteLine("✓ No hay productos bajo stock mínimo");
                    return;
                }

                Console.WriteLine($"⚠ {productosBajos.Count} producto(s) requieren reabastecimiento:\n");

                foreach (var producto in productosBajos)
                {
                    Console.WriteLine(producto.FormatearInfo());
                    Console.WriteLine($"   Stock actual: {producto.Stock} | Mínimo: {producto.StockMinimo}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        static void ProbarConexion()
        {
            Console.Clear();
            // Usar el nuevo método de diagnóstico detallado
            Console.WriteLine(DatabaseConfig.ObtenerDiagnosticoConexion());
        }

        // Método para mostrar menú de UI
        static void MostrarMenuUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║     SISTEMA DE VENTAS - DEMO DE UI     ║");
                Console.WriteLine("╚════════════════════════════════════════╝\n");
                Console.WriteLine("Seleccione el formulario a visualizar:\n");
                Console.WriteLine("  1. Formulario Principal (MDI)");
                Console.WriteLine("  2. Formulario de Categorías");
                Console.WriteLine("  3. Formulario de Productos");
                Console.WriteLine("  4. Formulario de Clientes");
                Console.WriteLine("  5. Formulario de Usuarios");
                Console.WriteLine("  6. Formulario de Ventas");
                Console.WriteLine("  0. Volver a menú principal\n");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();
                Console.Clear();

                switch (opcion)
                {
                    case "1":
                        AbrirFormPrincipal();
                        break;
                    case "2":
                        AbrirFormCategorias();
                        break;
                    case "3":
                        AbrirFormProductos();
                        break;
                    case "4":
                        AbrirFormClientes();
                        break;
                    case "5":
                        AbrirFormUsuarios();
                        break;
                    case "6":
                        AbrirFormVenta();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        System.Threading.Thread.Sleep(1500);
                        break;
                }
            }
        }

        static void AbrirFormPrincipal()
        {
            try
            {
                Application.Run(new FormPrincipal());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormPrincipal: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AbrirFormCategorias()
        {
            try
            {
                var form = new FormCategorias();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormCategorias: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AbrirFormProductos()
        {
            try
            {
                var form = new FormProductos();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormProductos: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AbrirFormClientes()
        {
            try
            {
                var form = new FormClientes();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormClientes: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AbrirFormUsuarios()
        {
            try
            {
                var form = new FormUsuarios();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormUsuarios: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void AbrirFormVenta()
        {
            try
            {
                var form = new FormVenta();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al abrir FormVenta: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}

