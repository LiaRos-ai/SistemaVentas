-- ============================================
-- SISTEMA DE VENTAS - BASE DE DATOS
-- Versión: 1.0
-- Fecha: Primera Semana
-- ============================================

-- Crear base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SistemaVentas')
BEGIN
    CREATE DATABASE SistemaVentas;
END
GO

USE SistemaVentas;
GO

-- ============================================
-- TABLA: Categorias
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categorias')
BEGIN
    CREATE TABLE Categorias (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(50) NOT NULL UNIQUE,
        Descripcion NVARCHAR(200) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- ============================================
-- TABLA: Productos
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
BEGIN
    CREATE TABLE Productos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Codigo NVARCHAR(20) NOT NULL UNIQUE,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NULL,
        PrecioCompra DECIMAL(10,2) NOT NULL CHECK (PrecioCompra > 0),
        PrecioVenta DECIMAL(10,2) NOT NULL CHECK (PrecioVenta > 0),
        Stock INT NOT NULL DEFAULT 0 CHECK (Stock >= 0),
        StockMinimo INT NOT NULL DEFAULT 5,
        CategoriaId INT NOT NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT FK_Productos_Categorias 
            FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
    );
    
    CREATE INDEX IX_Productos_Nombre ON Productos(Nombre);
    CREATE INDEX IX_Productos_Codigo ON Productos(Codigo);
    CREATE INDEX IX_Productos_CategoriaId ON Productos(CategoriaId);
END
GO

-- ============================================
-- TABLA: Clientes
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
BEGIN
    CREATE TABLE Clientes (
        Id INT PRIMARY KEY IDENTITY(1,1),
        TipoDocumento TINYINT NOT NULL, -- 1=DNI, 2=RUC, 3=Carnet, 4=Pasaporte
        NumeroDocumento NVARCHAR(20) NOT NULL UNIQUE,
        Nombres NVARCHAR(100) NOT NULL,
        Apellidos NVARCHAR(100) NOT NULL,
        Direccion NVARCHAR(200) NULL,
        Telefono NVARCHAR(15) NULL,
        Email NVARCHAR(100) NULL,
        Activo BIT NOT NULL DEFAULT 1,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT CHK_TipoDocumento CHECK (TipoDocumento BETWEEN 1 AND 4)
    );
    
    CREATE INDEX IX_Clientes_NumeroDocumento ON Clientes(NumeroDocumento);
    CREATE INDEX IX_Clientes_Nombres ON Clientes(Nombres, Apellidos);
END
GO

-- ============================================
-- TABLA: Usuarios
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id INT PRIMARY KEY IDENTITY(1,1),
        NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
        Clave NVARCHAR(255) NOT NULL,
        Nombres NVARCHAR(100) NOT NULL,
        Apellidos NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NULL,
        EsAdministrador BIT NOT NULL DEFAULT 0,
        Activo BIT NOT NULL DEFAULT 1,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
    );
    
    CREATE INDEX IX_Usuarios_NombreUsuario ON Usuarios(NombreUsuario);
END
GO

-- ============================================
-- TABLA: Ventas
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ventas')
BEGIN
    CREATE TABLE Ventas (
        Id INT PRIMARY KEY IDENTITY(1,1),
        NumeroVenta NVARCHAR(20) NOT NULL UNIQUE,
        ClienteId INT NOT NULL,
        UsuarioId INT NOT NULL,
        FechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
        SubTotal DECIMAL(10,2) NOT NULL,
        Impuesto DECIMAL(10,2) NOT NULL,
        Total DECIMAL(10,2) NOT NULL,
        Estado TINYINT NOT NULL DEFAULT 1, -- 1=Pendiente, 2=Completada, 3=Anulada
        
        CONSTRAINT FK_Ventas_Clientes 
            FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
        CONSTRAINT FK_Ventas_Usuarios 
            FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
        CONSTRAINT CHK_Estado CHECK (Estado BETWEEN 1 AND 3)
    );
    
    CREATE INDEX IX_Ventas_FechaVenta ON Ventas(FechaVenta);
    CREATE INDEX IX_Ventas_NumeroVenta ON Ventas(NumeroVenta);
    CREATE INDEX IX_Ventas_ClienteId ON Ventas(ClienteId);
END
GO

-- ============================================
-- TABLA: DetalleVenta
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetalleVenta')
BEGIN
    CREATE TABLE DetalleVenta (
        Id INT PRIMARY KEY IDENTITY(1,1),
        VentaId INT NOT NULL,
        ProductoId INT NOT NULL,
        Cantidad INT NOT NULL CHECK (Cantidad > 0),
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        SubTotal DECIMAL(10,2) NOT NULL,
        
        CONSTRAINT FK_DetalleVenta_Ventas 
            FOREIGN KEY (VentaId) REFERENCES Ventas(Id) ON DELETE CASCADE,
        CONSTRAINT FK_DetalleVenta_Productos 
            FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
    );
    
    CREATE INDEX IX_DetalleVenta_VentaId ON DetalleVenta(VentaId);
    CREATE INDEX IX_DetalleVenta_ProductoId ON DetalleVenta(ProductoId);
END
GO

-- ============================================
-- DATOS DE PRUEBA
-- ============================================

-- Categorías
IF NOT EXISTS (SELECT * FROM Categorias)
BEGIN
    INSERT INTO Categorias (Nombre, Descripcion) VALUES
    ('Electrónica', 'Productos electrónicos y tecnología'),
    ('Ropa', 'Prendas de vestir'),
    ('Alimentos', 'Productos comestibles'),
    ('Hogar', 'Artículos para el hogar'),
    ('Deportes', 'Equipamiento deportivo');
END
GO

-- Productos
IF NOT EXISTS (SELECT * FROM Productos)
BEGIN
    INSERT INTO Productos (Codigo, Nombre, Descripcion, PrecioCompra, PrecioVenta, Stock, StockMinimo, CategoriaId) VALUES
    ('PROD001', 'Laptop HP 15', 'Laptop HP Core i5, 8GB RAM, 256GB SSD', 800.00, 1200.00, 15, 5, 1),
    ('PROD002', 'Mouse Logitech', 'Mouse inalámbrico Logitech M170', 20.00, 35.00, 50, 10, 1),
    ('PROD003', 'Teclado Mecánico', 'Teclado mecánico RGB retroiluminado', 50.00, 85.00, 30, 8, 1),
    ('PROD004', 'Monitor Samsung 24"', 'Monitor LED Full HD 24 pulgadas', 240.00, 350.00, 20, 5, 1),
    ('PROD005', 'Camisa Polo', 'Camisa polo de algodón', 15.00, 35.00, 40, 10, 2),
    ('PROD006', 'Pantalón Jean', 'Pantalón jean azul clásico', 25.00, 55.00, 35, 10, 2),
    ('PROD007', 'Arroz 1kg', 'Arroz blanco de primera calidad', 1.50, 2.50, 200, 50, 3),
    ('PROD008', 'Aceite 1L', 'Aceite vegetal comestible', 3.00, 5.50, 150, 30, 3),
    ('PROD009', 'Juego de Sábanas', 'Juego de sábanas matrimoniales', 30.00, 60.00, 25, 8, 4),
    ('PROD010', 'Balón de Fútbol', 'Balón oficial de fútbol', 20.00, 40.00, 30, 10, 5);
END
GO

-- Usuarios (Contraseña: 123456)
IF NOT EXISTS (SELECT * FROM Usuarios)
BEGIN
    INSERT INTO Usuarios (NombreUsuario, Clave, Nombres, Apellidos, Email, EsAdministrador) VALUES
    ('admin', '$2a$11$p7Kzy3QXQKz7qQVKKkGp6.JK5wz5QXBvI1KPX3FQP3kB1Z6wvNM5W', 'Administrador', 'Sistema', 'admin@sistema.com', 1),
    ('vendedor1', '$2a$11$p7Kzy3QXQKz7qQVKKkGp6.JK5wz5QXBvI1KPX3FQP3kB1Z6wvNM5W', 'Juan', 'Pérez', 'juan@sistema.com', 0),
    ('vendedor2', '$2a$11$p7Kzy3QXQKz7qQVKKkGp6.JK5wz5QXBvI1KPX3FQP3kB1Z6wvNM5W', 'María', 'González', 'maria@sistema.com', 0);
END
GO

-- Clientes
IF NOT EXISTS (SELECT * FROM Clientes)
BEGIN
    INSERT INTO Clientes (TipoDocumento, NumeroDocumento, Nombres, Apellidos, Direccion, Telefono, Email) VALUES
    (1, '12345678', 'Carlos', 'López', 'Av. Principal 123', '71234567', 'carlos@email.com'),
    (2, '20123456789', 'Ana', 'Martínez', 'Calle Comercio 456', '72345678', 'ana@empresa.com'),
    (1, '87654321', 'Pedro', 'Ramírez', 'Zona Sur 789', '73456789', 'pedro@email.com'),
    (1, '11223344', 'Laura', 'Sánchez', 'Av. 6 de Agosto 321', '74567890', 'laura@email.com'),
    (3, '1234567', 'José', 'Torres', 'Calle Libertad 654', '75678901', 'jose@email.com');
END
GO

-- ============================================
-- PROCEDIMIENTOS ALMACENADOS
-- ============================================

-- Procedimiento: Registrar Venta
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_RegistrarVenta')
    DROP PROCEDURE sp_RegistrarVenta;
GO

CREATE PROCEDURE sp_RegistrarVenta
    @ClienteId INT,
    @UsuarioId INT,
    @DetallesJSON NVARCHAR(MAX),
    @VentaId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @SubTotal DECIMAL(10,2) = 0;
        DECLARE @Impuesto DECIMAL(10,2) = 0;
        DECLARE @Total DECIMAL(10,2) = 0;
        DECLARE @NumeroVenta NVARCHAR(20);
        
        -- Generar número de venta
        DECLARE @Contador INT;
        SELECT @Contador = ISNULL(MAX(Id), 0) + 1 FROM Ventas;
        SET @NumeroVenta = 'V-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-' + FORMAT(@Contador, 'D6');
        
        -- Insertar venta
        INSERT INTO Ventas (NumeroVenta, ClienteId, UsuarioId, FechaVenta, SubTotal, Impuesto, Total, Estado)
        VALUES (@NumeroVenta, @ClienteId, @UsuarioId, GETDATE(), 0, 0, 0, 2); -- Estado Completada
        
        SET @VentaId = SCOPE_IDENTITY();
        
        -- Insertar detalles (aquí deberías parsear el JSON)
        -- Por simplicidad, este ejemplo asume que los detalles se insertan por separado
        
        -- Calcular totales
        SELECT @SubTotal = SUM(SubTotal) FROM DetalleVenta WHERE VentaId = @VentaId;
        SET @Impuesto = @SubTotal * 0.13; -- 13% IVA
        SET @Total = @SubTotal + @Impuesto;
        
        -- Actualizar totales
        UPDATE Ventas 
        SET SubTotal = @SubTotal, Impuesto = @Impuesto, Total = @Total
        WHERE Id = @VentaId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ============================================
-- VISTAS
-- ============================================

-- Vista: Productos con Categoría
IF EXISTS (SELECT * FROM sys.views WHERE name = 'v_ProductosConCategoria')
    DROP VIEW v_ProductosConCategoria;
GO

CREATE VIEW v_ProductosConCategoria
AS
SELECT 
    p.Id,
    p.Codigo,
    p.Nombre,
    p.Descripcion,
    p.PrecioCompra,
    p.PrecioVenta,
    p.Stock,
    p.StockMinimo,
    c.Nombre AS CategoriaNombre,
    CASE 
        WHEN p.Stock <= p.StockMinimo THEN 'Bajo Stock'
        WHEN p.Stock <= p.StockMinimo * 2 THEN 'Stock Medio'
        ELSE 'Stock OK'
    END AS EstadoStock,
    p.Activo
FROM Productos p
INNER JOIN Categorias c ON p.CategoriaId = c.Id
WHERE p.Activo = 1;
GO

-- Vista: Ventas Completas
IF EXISTS (SELECT * FROM sys.views WHERE name = 'v_VentasCompletas')
    DROP VIEW v_VentasCompletas;
GO

CREATE VIEW v_VentasCompletas
AS
SELECT 
    v.Id,
    v.NumeroVenta,
    v.FechaVenta,
    c.Nombres + ' ' + c.Apellidos AS Cliente,
    c.NumeroDocumento,
    u.NombreUsuario AS Vendedor,
    v.SubTotal,
    v.Impuesto,
    v.Total,
    CASE v.Estado
        WHEN 1 THEN 'Pendiente'
        WHEN 2 THEN 'Completada'
        WHEN 3 THEN 'Anulada'
    END AS EstadoVenta
FROM Ventas v
INNER JOIN Clientes c ON v.ClienteId = c.Id
INNER JOIN Usuarios u ON v.UsuarioId = u.Id;
GO

PRINT 'Base de datos creada exitosamente';
GO

