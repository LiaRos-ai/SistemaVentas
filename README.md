# 📊 Sistema de Ventas

Sistema de gestión de ventas desarrollado en **.NET 10** con una arquitectura en capas que incluye entidades, repositorios y extensiones de negocio.

---

## 📁 Estructura del Proyecto

```
SistemaVentas/
├── 📂 Entidades/                    # Modelos de datos
│   ├── Cliente.cs                   # Información de clientes
│   ├── Producto.cs                  # Catálogo de productos
│   ├── Categoria.cs                 # Categorías de productos
│   ├── Venta.cs                     # Registro de ventas
│   ├── DetalleVenta.cs              # Detalles de cada venta
│   ├── Usuario.cs                   # Usuarios del sistema
│   └── Enums/
│       ├── EstadoVenta.cs           # Estados: Pendiente, Completada, Cancelada
│       └── TipoDocumento.cs         # Tipos de documento: RUC, DNI, etc.
│
├── 📂 DAL/                          # Data Access Layer
│   ├── Conexion/
│   │   └── DatabaseConfig.cs        # Configuración de conexión a SQL Server
│   └── Repositories/
│       ├── BaseRepository.cs        # Clase base para repos (genérica)
│       ├── ProductRepository.cs     # CRUD de productos
│       └── CategoriaRepository.cs   # CRUD de categorías
│
├── 📂 Negocio/                      # Business Logic Layer
│   └── Extensions/
│       ├── StringExtensions.cs      # Métodos de extensión para string
│       ├── ProductExtensions.cs     # Lógica de negocio para productos
│       ├── ClienteExtensions.cs     # Lógica de negocio para clientes
│       └── VentaExtensions.cs       # Lógica de negocio para ventas
│
├── 📂 ConsoleTest/                  # Aplicación de consola de prueba
│   ├── Program.cs                   # Punto de entrada y menú interactivo
│   └── App.config                   # Configuración (cadena conexión, etc.)
│
└── 📄 README.md                     # Este archivo
```

---

## 🎯 Funciones Principales

### 1. **Gestión de Productos (CRUD)**
- 📝 **Listar**: Visualizar todos los productos registrados
- ➕ **Crear**: Insertar nuevo producto con código, nombre, precio, stock, etc.
- 🔍 **Buscar**: Encontrar producto por nombre
- ✏️ **Actualizar**: Modificar datos de un producto existente
- 🗑️ **Eliminar**: Dar de baja productos

### 2. **Gestión de Categorías**
- Organizar productos por categorías
- Consultar productos por categoría
- Validar categoría al crear productos

### 3. **Control de Stock**
- Monitorear productos con stock bajo
- Alertas automáticas cuando se alcanza el stock mínimo
- Historial de movimientos de inventario

### 4. **Gestión de Ventas**
- Crear nuevas ventas con detalles
- Registrar clientes
- Calcular totales automáticamente
- Estados de venta (Pendiente, Completada, Cancelada)

### 5. **Extensiones de Negocio**
- Métodos de extensión para validaciones
- Formateo de datos
- Lógica de cálculos comerciales

---

## 🛠️ Requisitos Previos

- **.NET 10** o superior
- **SQL Server** (LocalDB o remoto)
- **Visual Studio 2026** (recomendado)
- Conexión a base de datos configurada

---

## 🚀 Pasos de Ejecución

### 1️⃣ **Configuración de Base de Datos**

#### Opción A: LocalDB (Windows)
```
<!-- ConsoleTest\App.config -->
<add name="SistemaVentas"
     connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SistemaVentas;Integrated Security=True;TrustServerCertificate=True;"
     providerName="System.Data.SqlClient" />
```

#### Opción B: SQL Server con Usuario/Contraseña
```
<add name="SistemaVentas"
     connectionString="Data Source=localhost;Initial Catalog=SistemaVentas;User ID=sa;Password=tu_contraseña;TrustServerCertificate=True;"
     providerName="System.Data.SqlClient" />
```

#### Opción C: Servidor Remoto
```
<add name="SistemaVentas"
     connectionString="Data Source=192.168.1.100;Initial Catalog=SistemaVentas;Integrated Security=True;TrustServerCertificate=True;"
     providerName="System.Data.SqlClient" />
```

### 2️⃣ **Crear la Base de Datos**

Ejecutar el script SQL siguiente en SQL Server Management Studio:

```sql
-- Crear base de datos
CREATE DATABASE SistemaVentas;
GO

USE SistemaVentas;
GO

-- Tabla de Categorías
CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

-- Tabla de Productos
CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Codigo NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(255),
    CategoriaId INT NOT NULL,
    PrecioCompra DECIMAL(10,2) NOT NULL,
    PrecioVenta DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    StockMinimo INT NOT NULL DEFAULT 5,
    Activo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);

-- Tabla de Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    TipoDocumento INT,
    NumeroDocumento NVARCHAR(50),
    Activo BIT NOT NULL DEFAULT 1
);

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Email NVARCHAR(100),
    Activo BIT NOT NULL DEFAULT 1
);

-- Tabla de Ventas
CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    UsuarioId INT NOT NULL,
    FechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    Estado INT NOT NULL DEFAULT 1,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- Tabla de Detalles de Venta
CREATE TABLE DetallesVenta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    VentaId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VentaId) REFERENCES Ventas(Id),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);
```

### 3️⃣ **Compilar el Proyecto**

```bash
# En Visual Studio: Ctrl + Shift + B
# O en consola:
dotnet build
```

### 4️⃣ **Ejecutar la Aplicación**

```bash
# En Visual Studio: F5 o Ctrl + F5
# O en consola:
dotnet run --project ConsoleTest
```

### 5️⃣ **Menú Principal**

Al ejecutarse la consola, verá:

```
╔════════════════════════════════════════════════╗
║     SISTEMA DE VENTAS - DEMO                   ║
╚════════════════════════════════════════════════╝

  CRUD DE PRODUCTOS:
  1. Listar todos los productos
  2. Insertar nuevo producto
  3. Buscar producto por nombre
  4. Actualizar producto
  5. Eliminar producto
  6. Demostrar extension methods
  7. Listar categorías
  8. Productos con bajo stock
  9. Probar conexión
  0. Salir
```

---

## 📚 Capa de Datos (DAL)

### `DatabaseConfig.cs`
Gestiona la conexión a SQL Server:
- Lee la cadena de conexión desde `App.config`
- Valida la conexión a la base de datos
- Crea instancias de conexión seguras

### `BaseRepository.cs`
Clase base genérica para todos los repositorios:
- `EjecutarComando()`: Ejecuta INSERT, UPDATE, DELETE
- `EjecutarEscalar<T>()`: Obtiene un valor único
- `EjecutarLectura()`: Ejecuta SELECT con DataReader

### `ProductRepository.cs`
Operaciones CRUD de productos:
- `ObtenerTodos()`: Lista todos los productos
- `ObtenerPorId()`: Busca producto por ID
- `ObtenerPorNombre()`: Busca producto por nombre
- `Insertar()`: Crea nuevo producto
- `Actualizar()`: Modifica producto
- `Eliminar()`: Da de baja producto

### `CategoriaRepository.cs`
Operaciones CRUD de categorías

---

## 🧠 Capa de Negocio (Negocio)

### Extensiones (`Extensions/`)
Métodos de extensión para lógica empresarial:

```csharp
// ProductExtensions.cs
producto.EstaActivo();              // Valida si está activo
producto.TieneBajoStock();           // Verifica stock mínimo
producto.CalcularMargen();           // Calcula margen de ganancia

// ClienteExtensions.cs
cliente.TieneDocumentoValido();      // Valida documento
cliente.ObtenerNombreCompleto();     // Formatea nombre

// VentaExtensions.cs
venta.CalcularTotal();               // Suma detalles
venta.EstaCompletada();              // Verifica estado

// StringExtensions.cs
texto.EstaVacio();                   // Valida cadena
texto.EscaparComillas();             // Escapa caracteres SQL
```

---

## 🧪 Pruebas Incluidas

En `Program.cs` encontrará funciones de demostración:

- **ProbarConexion()**: Verifica conexión a DB
- **ListarProductos()**: Muestra todos los productos
- **BuscarProducto()**: Búsqueda por nombre con filtros
- **ProductosBajoStock()**: Alerta de bajo inventario
- **DemostrarExtensionMethods()**: Prueba lógica de negocio

---

## 🔒 Seguridad

- ✅ Conexión con `TrustServerCertificate=True` para certificados auto-firmados
- ✅ Autenticación integrada de Windows cuando es posible
- ✅ Parámetros SQL seguros (sin concatenación de strings)
- ⚠️ Guarde contraseñas en `App.config` en entorno de producción (considere Azure Key Vault)

---

## 📋 Notas Adicionales

- La aplicación prueba la conexión antes de iniciar
- Los productos tienen control de stock mínimo
- Cada categoría puede tener múltiples productos
- Las ventas registran fecha, cliente y usuario
- Estados de venta: `Pendiente (1)`, `Completada (2)`, `Cancelada (3)`

---

## Soporte

Para resolver problemas:

1. **Error de conexión**: Verifique `App.config` y la base de datos
2. **Tablas no existen**: Ejecute el script SQL incluido
3. **Permisos**: Asegúrese de tener acceso a SQL Server

---

**Versión**: 1.0  
**Última actualización**: enero 2026  
**Desarrollado con**: .NET 10, C#, SQL Server
