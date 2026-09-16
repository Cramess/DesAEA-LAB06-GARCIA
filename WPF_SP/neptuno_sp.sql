USE NeptunoDB;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Categorias' AND COLUMN_NAME = 'Activo')
BEGIN
    ALTER TABLE dbo.Categorias ADD Activo BIT NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Proveedores' AND COLUMN_NAME = 'Activo')
BEGIN
    ALTER TABLE dbo.Proveedores ADD Activo BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Productos' AND COLUMN_NAME = 'Activo')
BEGIN
    ALTER TABLE dbo.Productos ADD Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pedidos' AND COLUMN_NAME = 'Activo')
BEGIN
    ALTER TABLE dbo.Pedidos ADD Activo BIT NOT NULL CONSTRAINT DF_Pedidos_Activo DEFAULT 1;
END
GO

-- CRUD Categorias
CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion, Activo
    FROM dbo.Categorias
    WHERE Activo = 1
    ORDER BY NombreCategoria ASC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_ObtenerPorId
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion, Activo
    FROM dbo.Categorias
    WHERE CategoriaID = @CategoriaID AND Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_Crear
    @NombreCategoria NVARCHAR(30),
    @Descripcion     NVARCHAR(200) = NULL,
    @NuevoID         INT           = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Categorias (NombreCategoria, Descripcion, Activo)
        VALUES (@NombreCategoria, @Descripcion, 1);

        SET @NuevoID = SCOPE_IDENTITY();
        SELECT @NuevoID AS CategoriaID;
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
        WHERE CategoriaID = @CategoriaID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50001, 'Categoría no encontrada o ya se encuentra inactiva.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categoria_EliminarLogico
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Categorias
        SET Activo = 0
        WHERE CategoriaID = @CategoriaID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50002, 'Categoría no encontrada o ya fue desactivada.', 1;
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
    EXEC dbo.usp_Categoria_EliminarLogico @CategoriaID;
END
GO

-- CRUD Proveedores
CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax, Activo
    FROM dbo.Proveedores
    WHERE Activo = 1
    ORDER BY CompaniaNombre ASC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_ObtenerPorId
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax, Activo
    FROM dbo.Proveedores
    WHERE ProveedorID = @ProveedorID AND Activo = 1;
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
    @Fax            NVARCHAR(24) = NULL,
    @NuevoID        INT          = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Proveedores (CompaniaNombre, NombreContacto, CargoContacto,
                                     Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax, Activo)
        VALUES (@CompaniaNombre, @NombreContacto, @CargoContacto,
                @Direccion, @Ciudad, @CodigoPostal, @Pais, @Telefono, @Fax, 1);

        SET @NuevoID = SCOPE_IDENTITY();
        SELECT @NuevoID AS ProveedorID;
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
        WHERE ProveedorID = @ProveedorID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50003, 'Proveedor no encontrado o se encuentra inactivo.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_EliminarLogico
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Proveedores
        SET Activo = 0
        WHERE ProveedorID = @ProveedorID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50004, 'Proveedor no encontrado o ya fue desactivado.', 1;
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
    EXEC dbo.usp_Proveedor_EliminarLogico @ProveedorID;
END
GO

-- Busqueda de Proveedores por filtros
CREATE OR ALTER PROCEDURE dbo.usp_Proveedor_Buscar
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad         NVARCHAR(30) = NULL,
    @CompaniaNombre NVARCHAR(60) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto,
           Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax, Activo
    FROM dbo.Proveedores
    WHERE Activo = 1
      AND (@NombreContacto IS NULL OR LTRIM(RTRIM(@NombreContacto)) = '' OR NombreContacto LIKE N'%' + @NombreContacto + N'%')
      AND (@Ciudad         IS NULL OR LTRIM(RTRIM(@Ciudad))         = '' OR Ciudad         LIKE N'%' + @Ciudad + N'%')
      AND (@CompaniaNombre IS NULL OR LTRIM(RTRIM(@CompaniaNombre)) = '' OR CompaniaNombre LIKE N'%' + @CompaniaNombre + N'%')
    ORDER BY CompaniaNombre ASC;
END
GO

-- CRUD Productos
CREATE OR ALTER PROCEDURE dbo.usp_Producto_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado, p.Activo,
           ISNULL(c.NombreCategoria, N'-') AS NombreCategoria,
           ISNULL(pr.CompaniaNombre, N'-') AS NombreProveedor
    FROM dbo.Productos p
    LEFT JOIN dbo.Categorias c   ON c.CategoriaID  = p.CategoriaID
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    WHERE p.Activo = 1
    ORDER BY p.NombreProducto ASC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_ObtenerPorId
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID, p.NombreProducto, p.ProveedorID, p.CategoriaID,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia,
           p.UnidadesEnPedido, p.NivelDeReorden, p.Descontinuado, p.Activo,
           ISNULL(c.NombreCategoria, N'-') AS NombreCategoria,
           ISNULL(pr.CompaniaNombre, N'-') AS NombreProveedor
    FROM dbo.Productos p
    LEFT JOIN dbo.Categorias c   ON c.CategoriaID  = p.CategoriaID
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    WHERE p.ProductoID = @ProductoID AND p.Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Crear
    @NombreProducto       NVARCHAR(60),
    @ProveedorID          INT            = NULL,
    @CategoriaID          INT            = NULL,
    @CantidadPorUnidad    NVARCHAR(30)   = NULL,
    @PrecioUnidad         DECIMAL(10,2)  = 0,
    @UnidadesEnExistencia SMALLINT       = 0,
    @UnidadesEnPedido     SMALLINT       = 0,
    @NivelDeReorden       SMALLINT       = 0,
    @Descontinuado        BIT            = 0,
    @NuevoID              INT            = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Productos (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad,
                                   PrecioUnidad, UnidadesEnExistencia, UnidadesEnPedido,
                                   NivelDeReorden, Descontinuado, Activo)
        VALUES (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad,
                @PrecioUnidad, @UnidadesEnExistencia, @UnidadesEnPedido,
                @NivelDeReorden, @Descontinuado, 1);

        SET @NuevoID = SCOPE_IDENTITY();
        SELECT @NuevoID AS ProductoID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_Actualizar
    @ProductoID           INT,
    @NombreProducto       NVARCHAR(60),
    @ProveedorID          INT            = NULL,
    @CategoriaID          INT            = NULL,
    @CantidadPorUnidad    NVARCHAR(30)   = NULL,
    @PrecioUnidad         DECIMAL(10,2)  = 0,
    @UnidadesEnExistencia SMALLINT       = 0,
    @UnidadesEnPedido     SMALLINT       = 0,
    @NivelDeReorden       SMALLINT       = 0,
    @Descontinuado        BIT            = 0
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
        WHERE ProductoID = @ProductoID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50005, 'Producto no encontrado o se encuentra inactivo.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Producto_EliminarLogico
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Productos
        SET Activo = 0
        WHERE ProductoID = @ProductoID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50006, 'Producto no encontrado o ya fue desactivado.', 1;
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
    EXEC dbo.usp_Producto_EliminarLogico @ProductoID;
END
GO

-- CRUD Pedidos
CREATE OR ALTER PROCEDURE dbo.usp_Pedido_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.ClienteID, p.EmpleadoID, p.FechaPedido, p.FechaRequerida,
           p.FechaEnvio, p.TransportistaID, p.Destinatario, p.CiudadDestino, p.PaisDestino, p.Activo,
           ISNULL(c.Empresa, N'-') AS NombreCliente,
           ISNULL(e.Nombre + N' ' + e.Apellidos, N'-') AS NombreEmpleado,
           ISNULL(t.CompaniaNombre, N'-') AS NombreTransportista
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes       c ON c.ClienteID       = p.ClienteID
    LEFT JOIN dbo.Empleados      e ON e.EmpleadoID      = p.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = p.TransportistaID
    WHERE p.Activo = 1
    ORDER BY p.FechaPedido DESC, p.PedidoID DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_ObtenerPorId
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PedidoID, p.ClienteID, p.EmpleadoID, p.FechaPedido, p.FechaRequerida,
           p.FechaEnvio, p.TransportistaID, p.Destinatario, p.CiudadDestino, p.PaisDestino, p.Activo,
           ISNULL(c.Empresa, N'-') AS NombreCliente,
           ISNULL(e.Nombre + N' ' + e.Apellidos, N'-') AS NombreEmpleado,
           ISNULL(t.CompaniaNombre, N'-') AS NombreTransportista
    FROM dbo.Pedidos p
    LEFT JOIN dbo.Clientes       c ON c.ClienteID       = p.ClienteID
    LEFT JOIN dbo.Empleados      e ON e.EmpleadoID      = p.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = p.TransportistaID
    WHERE p.PedidoID = @PedidoID AND p.Activo = 1;
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
    @PaisDestino     NVARCHAR(30)  = NULL,
    @NuevoID         INT           = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Pedidos (ClienteID, EmpleadoID, FechaPedido, FechaRequerida,
                                 FechaEnvio, TransportistaID, Destinatario, CiudadDestino, PaisDestino, Activo)
        VALUES (@ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida,
                @FechaEnvio, @TransportistaID, @Destinatario, @CiudadDestino, @PaisDestino, 1);

        SET @NuevoID = SCOPE_IDENTITY();
        SELECT @NuevoID AS PedidoID;
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
        WHERE PedidoID = @PedidoID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50007, 'Pedido no encontrado o se encuentra inactivo.', 1;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Pedido_EliminarLogico
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Pedidos
        SET Activo = 0
        WHERE PedidoID = @PedidoID AND Activo = 1;

        IF @@ROWCOUNT = 0
            THROW 50008, 'Pedido no encontrado o ya fue desactivado.', 1;
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
    EXEC dbo.usp_Pedido_EliminarLogico @PedidoID;
END
GO

-- Reporte de detalle de pedidos por rango de fechas
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
        ISNULL(c.Empresa, N'-') AS NombreCliente,
        pr.NombreProducto,
        dp.PrecioUnidad,
        dp.Cantidad,
        dp.Descuento,
        CAST(dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento) AS DECIMAL(12,2)) AS SubTotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Pedidos   p  ON p.PedidoID   = dp.PedidoID
    INNER JOIN dbo.Productos pr ON pr.ProductoID = dp.ProductoID
    LEFT  JOIN dbo.Clientes  c  ON c.ClienteID   = p.ClienteID
    WHERE p.Activo = 1
      AND p.FechaPedido BETWEEN @FechaInicio AND @FechaFin
    ORDER BY p.FechaPedido DESC, dp.PedidoID DESC, pr.NombreProducto ASC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_DetallePedidos_ListarPorRangoFecha
    @FechaInicio DATE,
    @FechaFin    DATE
AS
BEGIN
    EXEC dbo.usp_DetallePedidos_ReportePorFechas @FechaInicio, @FechaFin;
END
GO

-- Listados auxiliares
CREATE OR ALTER PROCEDURE dbo.usp_Cliente_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ClienteID, Empresa, NombreContacto, Ciudad, Pais, Telefono
    FROM dbo.Clientes
    ORDER BY Empresa ASC;
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
    ORDER BY Apellidos ASC;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Transportista_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransportistaID, CompaniaNombre, Telefono
    FROM dbo.Transportistas
    ORDER BY CompaniaNombre ASC;
END
GO
