# 📊 Sistema de Ventas

Sistema de gestión de ventas desarrollado en **.NET 10** con una arquitectura en capas que incluye entidades, repositorios y extensiones de negocio.

---

## 📌 Cambios recientes (para revisión)
A continuación se describen los cambios implementados que deben ser revisados por el equipo:

- Capa de Lógica de Negocio
  - Se agregó la capa de negocio para centralizar reglas y procesos que antes residían parcialmente en la UI o los repositorios.
  - Responsabilidades: orquestar llamadas a repositorios, aplicar validaciones de negocio, manejar transacciones y preparar DTOs para la UI.

- Interfaces de servicios definidas
  - Se definieron interfaces para los servicios principales (ej.: `IUsuarioService`, `IClienteService`, `IProductoService`, `IVentaService`).
  - Objetivo: desacoplar la UI y los controladores de la implementación concreta y facilitar pruebas unitarias y mocking.

- DTOs creados y utilizados
  - Se introdujeron DTOs para separar las entidades del modelo de datos de los objetos consumidos por la UI y las APIs internas.
  - Ejemplos: `ClienteDto`, `ProductoDto`, `VentaDto`, `DetalleVentaDto`, `UsuarioDto`.
  - Los DTOs contienen sólo la información necesaria para la presentación y evitan exponer lógica o campos sensibles.

- Validaciones de negocio implementadas
  - Validaciones centrales movidas a la capa de negocio (p. ej. duplicados, verificación de stock, validación de precios, cliente activo, permisos de usuario).
  - Errores de validación devuelven mensajes claros y consistentes para la UI.

- Transacciones funcionando correctamente
  - Operaciones compuestas (por ejemplo: creación de una venta + decremento de stock) se ejecutan dentro de una transacción atómica.
  - En caso de error, la transacción se revierte para mantener la integridad de los datos.
  - La coordinación puede residir en `VentaService` o en el repositorio específico que soporte transacciones.

- StatusBar informativo (usuario, fecha, hora)
  - La barra de estado en `FormPrincipal` muestra el usuario autenticado, la fecha y la hora actual.
  - Se actualiza en tiempo real (formato: `Usuario: <nombre> | Fecha: YYYY-MM-DD | Hora: HH:MM:SS`) y refresca automáticamente cada segundo/minuto según configuración.

¿Dónde revisar los cambios?
- `Negocio/` - implementación de servicios, validaciones y orquestación
- `Negocio/Interfaces/` - contratos de servicios (interfaces)
- `DTOs/` o `Negocio/DTOs/` - definiciones de objetos de transferencia
- `DAL/Repositories/` - transacciones y operaciones de bajo nivel (p. ej. `VentaRepository`)
- `UI/FormPrincipal.cs` - StatusBar y mecanismos de refresco
- Archivos de tests (si existen) para validar reglas y transacciones

Recomendación para la revisión:
1. Revisar las interfaces en `Negocio/Interfaces/` y la implementación en `Negocio/Services/`.
2. Verificar las validaciones unitarias y/o manualmente reproducir escenarios (venta con stock insuficiente, cliente inactivo, datos duplicados).
3. Ejecutar una venta de prueba y corroborar que la base de datos queda consistente en error y en éxito.
4. Abrir la UI y confirmar que la `StatusBar` muestra usuario, fecha y hora correctamente.

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
│   ├── Interfaces/                  # Contratos de servicios (IClienteService, ...)
│   ├── Services/                    # Implementaciones de servicios
│   └── Extensions/
│       ├── StringExtensions.cs      # Métodos de extensión para string
│       ├── ProductExtensions.cs     # Lógica de negocio para productos
│       ├── ClienteExtensions.cs     # Lógica de negocio para clientes
│       └── VentaExtensions.cs       # Lógica de negocio para ventas
│
├── 📂 DTOs/                         # Objetos de transferencia
│   ├── ClienteDto.cs
│   ├── ProductoDto.cs
│   └── VentaDto.cs
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

### 4️⃣ **Ejecutar la Aplicación - OPCIÓN 1: Interfaz Gráfica (Recomendado)**

```bash
# Directamente desde Visual Studio: F5 o Ctrl + F5
# O desde consola:
dotnet run --project SistemaVentas.csproj
```

**Verá el menú de selección de formularios:**

```
╔════════════════════════════════════════════╗
║     SISTEMA DE VENTAS - DEMO DE UI        ║
╚════════════════════════════════════════════╝

Seleccione el formulario a visualizar:

  1. Formulario Principal (MDI)
  2. Formulario de Categorías
  3. Formulario de Productos
  4. Formulario de Clientes
  5. Formulario de Usuarios
  6. Formulario de Ventas
  0. Volver a menú principal
```

**Seleccione cualquier opción para ver los datos en tiempo real desde la BD.**

### 5️⃣ **Ejecutar la Aplicación - OPCIÓN 2: Consola Interactiva (Legacy)**

```bash
# Si desea usar la consola interactiva anterior:
dotnet run --project ConsoleTest
```

**Verá el menú de consola:**

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

## 🎨 Interfaz Gráfica (UI)

### ✨ Formularios WinForms con Datos en Tiempo Real

A partir de la versión 1.1, la aplicación incluye una interfaz gráfica completa con formularios que cargan datos directamente desde la base de datos:

#### **FormPrincipal.cs** - Ventana MDI Principal
- Menú principal con acceso a todos los módulos
- Barra de herramientas con acciones rápidas
- Barra de estado con información del usuario
- Integración con la capa de negocio

#### **FormCategorias.cs** - Gestión de Categorías
- 📋 **DataGridView** que lista todas las categorías activas
- 🔘 Botones: **Nuevo**, **Editar**, **Eliminar**, **Recargar**
- 📊 Información total en pie de página
- 🎨 Panel de título con fondo azul personalizado

#### **FormProductos.cs** - Gestión de Productos
- 📋 **DataGridView** con lista completa de productos
- 🔍 **Búsqueda en tiempo real** por nombre
- 💰 **Formato de moneda** automático para precios
- 📊 Información dinámica de registros totales
- 📐 Columnas redimensionables: ID, Código, Nombre, Precio, Stock

#### **FormClientes.cs** - Gestión de Clientes
- 👤 **Lista completa de clientes activos**
- 📊 Muestra: ID, Nombres, Apellidos, Documento, Teléfono, Email
- 🔘 Botones de acción: Nuevo, Editar, Eliminar, Recargar
- 🎨 Panel de título con fondo verde personalizado

#### **FormUsuarios.cs** - Gestión de Usuarios
- 👨‍💼 **Listado de usuarios del sistema**
- 📋 Columnas: ID, Nombres, Apellidos, NombreUsuario, Email, EsAdministrador
- 🔄 Botón Recargar para actualizar en tiempo real
- 🎨 Panel de título con fondo púrpura personalizado

#### **FormVenta.cs** - Gestión de Ventas
- 📈 **Listado completo de ventas**
- 🔍 **Filtro por estado**: Todos, Pendiente, Completada, Anulada
- 💵 **Formato de moneda** en columna Total
- 📊 Información de ventas: NumeroVenta, FechaVenta, ClienteId, Total, Estado
- 🎨 Panel de título con fondo naranja personalizado

### 🎯 Características de UI
- ✅ **Temas personalizados** por formulario con colores distintos
- ✅ **DataGridView read-only** para seguridad de datos
- ✅ **Botones de recarga** para actualizar datos en tiempo real
- ✅ **Paneles organizados** por secciones (título, herramientas, datos, pie)
- ✅ **Formateo automático** de datos (moneda, fechas, enumeraciones)
- ✅ **Centrado en pantalla** de todos los formularios
- ✅ **Integración con ThemeHelper** para consistencia visual

---

## 🗄️ Repositorios DAL Ampliados

### `ClienteRepository.cs` ✨ NUEVO
Operaciones CRUD completas para clientes:
```csharp
// Métodos principales
List<Cliente> ObtenerTodos()          // Lista todos los clientes activos
Cliente ObtenerPorId(int id)          // Obtiene cliente por ID
int Insertar(Cliente cliente)         // Inserta nuevo cliente
bool Actualizar(Cliente cliente)      // Actualiza datos del cliente
bool Eliminar(int id)                 // Desactiva cliente (eliminación lógica)
List<Cliente> BuscarPorNombre(string nombre)  // Busca por nombre o apellido
```

Propiedades mapeadas: Id, TipoDocumento, NumeroDocumento, Nombres, Apellidos, Dirección, Teléfono, Email, Activo, FechaRegistro

### `UsuarioRepository.cs` ✨ NUEVO
Operaciones CRUD completas para usuarios:
```csharp
// Métodos principales
List<Usuario> ObtenerTodos()          // Lista todos los usuarios activos
Usuario ObtenerPorId(int id)          // Obtiene usuario por ID
Usuario ObtenerPorNombreUsuario(string nombre)  // Búsqueda por nombre usuario
int Insertar(Usuario usuario)         // Inserta nuevo usuario
bool Actualizar(Usuario usuario)      // Actualiza datos del usuario
bool Eliminar(int id)                 // Desactiva usuario (eliminación lógica)
```

Propiedades mapeadas: Id, NombreUsuario, Clave, Nombres, Apellidos, Email, EsAdministrador, Activo, FechaRegistro

### `VentaRepository.cs` ✨ NUEVO
Operaciones CRUD completas para ventas:
```csharp
// Métodos principales
List<Venta> ObtenerTodas()            // Lista todas las ventas
Venta ObtenerPorId(int id)            // Obtiene venta por ID
int Insertar(Venta venta)             // Inserta nueva venta
bool Actualizar(Venta venta)          // Actualiza datos de venta
bool Eliminar(int id)                 // Elimina venta
List<Venta> ObtenerPorEstado(EstadoVenta estado)  // Filtra por estado
```

Propiedades mapeadas: Id, NumeroVenta, ClienteId, UsuarioId, FechaVenta, SubTotal, Impuesto, Total, Estado

---

## 🚀 Nuevo Menú de UI - Punto de Entrada

El `Program.cs` ha sido actualizado para mostrar un **menú interactivo de demostración de UI**:

```
╔════════════════════════════════════════════╗
║     SISTEMA DE VENTAS - DEMO DE UI        ║
╚════════════════════════════════════════════╝

Seleccione el formulario a visualizar:

  1. Formulario Principal (MDI)
  2. Formulario de Categorías
  3. Formulario de Productos
  4. Formulario de Clientes
  5. Formulario de Usuarios
  6. Formulario de Ventas
  0. Volver a menú principal
```

Cada opción abre el formulario correspondiente **con datos cargados en tiempo real desde la base de datos**.

---

## 📊 Flujo de Datos

```
UI (WinForms)
    ↓
Formularios (FormXXX.cs)
    ↓
Repositorios (XXXRepository.cs)
    ↓
BaseRepository (Conexión a BD)
    ↓
SQL Server (BD SistemaVentas)
```

---

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
