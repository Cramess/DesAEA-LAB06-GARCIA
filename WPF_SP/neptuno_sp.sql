

USE NeptunoDB;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion
    FROM dbo.Categorias
    ORDER BY NombreCategoria;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_ObtenerPorId
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion
    FROM dbo.Categorias
    WHERE CategoriaID = @CategoriaID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Crear
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Categorias (NombreCategoria, Descripcion)
        VALUES (@NombreCategoria, @Descripcion);
        SELECT SCOPE_IDENTITY() AS CategoriaID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Actualizar
    @CategoriaID     INT,
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Categorias
        SET NombreCategoria = @NombreCategoria,
            Descripcion     = @Descripcion
        WHERE CategoriaID = @CategoriaID;
        IF @@ROWCOUNT = 0
            THROW 50001, 'Categoría no encontrada.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Eliminar
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.Categorias WHERE CategoriaID = @CategoriaID;
        IF @@ROWCOUNT = 0
            THROW 50002, 'Categoría no encontrada.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    ORDER BY CompaniaNombre;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_ObtenerPorId
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE ProveedorID = @ProveedorID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Crear
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @CargoContacto  NVARCHAR(40) = NULL,
    @Direccion      NVARCHAR(80) = NULL,
    @Ciudad         NVARCHAR(30) = NULL,
    @CodigoPostal   NVARCHAR(10) = NULL,
    @Pais           NVARCHAR(30) = NULL,
    @Telefono       NVARCHAR(24) = NULL,
    @Fax            NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Proveedores (CompaniaNombre, NombreContacto, CargoContacto,
                                     Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax)
        VALUES (@CompaniaNombre, @NombreContacto, @CargoContacto,
                @Direccion, @Ciudad, @CodigoPostal, @Pais, @Telefono, @Fax);
        SELECT SCOPE_IDENTITY() AS ProveedorID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Actualizar
    @ProveedorID    INT,
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @CargoContacto  NVARCHAR(40) = NULL,
    @Direccion      NVARCHAR(80) = NULL,
    @Ciudad         NVARCHAR(30) = NULL,
    @CodigoPostal   NVARCHAR(10) = NULL,
    @Pais           NVARCHAR(30) = NULL,
    @Telefono       NVARCHAR(24) = NULL,
    @Fax            NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Proveedores
        SET CompaniaNombre = @CompaniaNombre,
            NombreContacto = @NombreContacto,
            CargoContacto  = @CargoContacto,
            Direccion      = @Direccion,
            Ciudad         = @Ciudad,
            CodigoPostal   = @CodigoPostal,
            Pais           = @Pais,
            Telefono       = @Telefono,
            Fax            = @Fax
        WHERE ProveedorID = @ProveedorID;
        IF @@ROWCOUNT = 0
            THROW 50003, 'Proveedor no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Eliminar
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.Proveedores WHERE ProveedorID = @ProveedorID;
        IF @@ROWCOUNT = 0
            THROW 50004, 'Proveedor no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO


CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Buscar
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad         NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE (@NombreContacto IS NULL OR NombreContacto LIKE N'%' + @NombreContacto + N'%')
      AND (@Ciudad IS NULL OR Ciudad LIKE N'%' + @Ciudad + N'%')
    ORDER BY CompaniaNombre;
END
GO

-- 1. CRUD PRODUCTOS

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado,
           c.NombreCategoria,
           pr.CompaniaNombre AS NombreProveedor
    FROM dbo.Productos p
    LEFT JOIN dbo.Categorias c  ON c.CategoriaID  = p.CategoriaID
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    ORDER BY p.NombreProducto;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_ObtenerPorId
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado,
           c.NombreCategoria,
           pr.CompaniaNombre AS NombreProveedor
    FROM dbo.Productos p
    LEFT JOIN dbo.Categorias c  ON c.CategoriaID  = p.CategoriaID
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    WHERE p.ProductoID = @ProductoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Crear
    @NombreProducto      NVARCHAR(60),
    @ProveedorID         INT            = NULL,
    @CategoriaID         INT            = NULL,
    @CantidadPorUnidad   NVARCHAR(30)   = NULL,
    @PrecioUnidad        DECIMAL(10,2)  = 0,
    @UnidadesEnExistencia SMALLINT      = 0,
    @UnidadesEnPedido    SMALLINT       = 0,
    @NivelDeReorden      SMALLINT       = 0,
    @Descontinuado       BIT            = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Productos (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad,
                                   PrecioUnidad, UnidadesEnExistencia, UnidadesEnPedido,
                                   NivelDeReorden, Descontinuado)
        VALUES (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad,
                @PrecioUnidad, @UnidadesEnExistencia, @UnidadesEnPedido,
                @NivelDeReorden, @Descontinuado);
        SELECT SCOPE_IDENTITY() AS ProductoID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Actualizar
    @ProductoID          INT,
    @NombreProducto      NVARCHAR(60),
    @ProveedorID         INT            = NULL,
    @CategoriaID         INT            = NULL,
    @CantidadPorUnidad   NVARCHAR(30)   = NULL,
    @PrecioUnidad        DECIMAL(10,2)  = 0,
    @UnidadesEnExistencia SMALLINT      = 0,
    @UnidadesEnPedido    SMALLINT       = 0,
    @NivelDeReorden      SMALLINT       = 0,
    @Descontinuado       BIT            = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Productos
        SET NombreProducto       = @NombreProducto,
            ProveedorID          = @ProveedorID,
            CategoriaID          = @CategoriaID,
            CantidadPorUnidad    = @CantidadPorUnidad,
            PrecioUnidad         = @PrecioUnidad,
            UnidadesEnExistencia = @UnidadesEnExistencia,
            UnidadesEnPedido     = @UnidadesEnPedido,
            NivelDeReorden       = @NivelDeReorden,
            Descontinuado        = @Descontinuado
        WHERE ProductoID = @ProductoID;
        IF @@ROWCOUNT = 0
            THROW 50005, 'Producto no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Eliminar
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.Productos WHERE ProductoID = @ProductoID;
        IF @@ROWCOUNT = 0
            THROW 50006, 'Producto no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 2. CRUD PEDIDOS
CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.ClienteID, p.EmpleadoID, p.FechaPedido, p.FechaRequerida,
           p.FechaEnvio, p.TransportistaID, p.Destinatario, p.CiudadDestino, p.PaisDestino,
           c.Empresa        AS NombreCliente,
           e.Nombre + N' ' + e.Apellidos AS NombreEmpleado,
           t.CompaniaNombre AS NombreTransportista
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes       c ON c.ClienteID       = p.ClienteID
    LEFT JOIN dbo.Empleados      e ON e.EmpleadoID      = p.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = p.TransportistaID
    ORDER BY p.FechaPedido DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_ObtenerPorId
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.ClienteID, p.EmpleadoID, p.FechaPedido, p.FechaRequerida,
           p.FechaEnvio, p.TransportistaID, p.Destinatario, p.CiudadDestino, p.PaisDestino,
           c.Empresa        AS NombreCliente,
           e.Nombre + N' ' + e.Apellidos AS NombreEmpleado,
           t.CompaniaNombre AS NombreTransportista
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes       c ON c.ClienteID       = p.ClienteID
    LEFT JOIN dbo.Empleados      e ON e.EmpleadoID      = p.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = p.TransportistaID
    WHERE p.PedidoID = @PedidoID;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Crear
    @ClienteID       INT           = NULL,
    @EmpleadoID      INT           = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE          = NULL,
    @FechaEnvio      DATE          = NULL,
    @TransportistaID INT           = NULL,
    @Destinatario    NVARCHAR(60)  = NULL,
    @CiudadDestino   NVARCHAR(30)  = NULL,
    @PaisDestino     NVARCHAR(30)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Pedidos (ClienteID, EmpleadoID, FechaPedido, FechaRequerida,
                                 FechaEnvio, TransportistaID, Destinatario, CiudadDestino, PaisDestino)
        VALUES (@ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida,
                @FechaEnvio, @TransportistaID, @Destinatario, @CiudadDestino, @PaisDestino);
        SELECT SCOPE_IDENTITY() AS PedidoID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Actualizar
    @PedidoID        INT,
    @ClienteID       INT           = NULL,
    @EmpleadoID      INT           = NULL,
    @FechaPedido     DATE,
    @FechaRequerida  DATE          = NULL,
    @FechaEnvio      DATE          = NULL,
    @TransportistaID INT           = NULL,
    @Destinatario    NVARCHAR(60)  = NULL,
    @CiudadDestino   NVARCHAR(30)  = NULL,
    @PaisDestino     NVARCHAR(30)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Pedidos
        SET ClienteID       = @ClienteID,
            EmpleadoID      = @EmpleadoID,
            FechaPedido     = @FechaPedido,
            FechaRequerida  = @FechaRequerida,
            FechaEnvio      = @FechaEnvio,
            TransportistaID = @TransportistaID,
            Destinatario    = @Destinatario,
            CiudadDestino   = @CiudadDestino,
            PaisDestino     = @PaisDestino
        WHERE PedidoID = @PedidoID;
        IF @@ROWCOUNT = 0
            THROW 50007, 'Pedido no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Eliminar
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.DetallePedidos WHERE PedidoID = @PedidoID;
        DELETE FROM dbo.Pedidos         WHERE PedidoID = @PedidoID;
        IF @@ROWCOUNT = 0
            THROW 50008, 'Pedido no encontrado.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- 3. LISTADO AUXILIAR: CLIENTES, EMPLEADOS, TRANSPORTISTAS

CREATE OR ALTER PROCEDURE dbo.usp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ClienteID, Empresa, NombreContacto, Ciudad, Pais, Telefono
    FROM dbo.Clientes
    ORDER BY Empresa;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Empleado_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmpleadoID, Nombre, Apellidos,
           Nombre + N' ' + Apellidos AS NombreCompleto,
           Cargo
    FROM dbo.Empleados
    ORDER BY Apellidos;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Transportista_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransportistaID, CompaniaNombre, Telefono
    FROM dbo.Transportistas
    ORDER BY CompaniaNombre;
END
GO

-- 4. REPORTE: Detalle de Pedidos por Intervalo de Fechas

CREATE OR ALTER PROCEDURE dbo.usp_DetallePedidos_ReportePorFechas
    @FechaInicio DATE,
    @FechaFin    DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        dp.PedidoID,
        p.FechaPedido,
        p.FechaEnvio,
        p.Destinatario,
        p.CiudadDestino,
        p.PaisDestino,
        c.Empresa                    AS NombreCliente,
        pr.NombreProducto,
        dp.PrecioUnidad,
        dp.Cantidad,
        dp.Descuento,
        CAST(dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento) AS DECIMAL(12,2)) AS SubTotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Pedidos   p  ON p.PedidoID   = dp.PedidoID
    INNER JOIN dbo.Productos pr ON pr.ProductoID = dp.ProductoID
    LEFT  JOIN dbo.Clientes  c  ON c.ClienteID   = p.ClienteID
    WHERE p.FechaPedido BETWEEN @FechaInicio AND @FechaFin
    ORDER BY p.FechaPedido DESC, dp.PedidoID, pr.NombreProducto;
END
GO

-- 5. VERIFICACIÓN DE SPs CREADOS
SELECT ROUTINE_NAME, ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_NAME;
GO
