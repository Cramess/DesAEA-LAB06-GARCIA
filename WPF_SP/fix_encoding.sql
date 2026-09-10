/* ============================================================
   NeptunoDB - Corrección de datos con caracteres especiales
   ============================================================ */
USE NeptunoDB;
GO

-- Actualizar Categorías
UPDATE dbo.Categorias SET NombreCategoria = N'Bebidas',            Descripcion = N'Refrescos, cafés, tés, cervezas y otras bebidas' WHERE CategoriaID = 1;
UPDATE dbo.Categorias SET NombreCategoria = N'Condimentos',        Descripcion = N'Salsas, especias y aderezos'                       WHERE CategoriaID = 2;
UPDATE dbo.Categorias SET NombreCategoria = N'Confituras',         Descripcion = N'Mermeladas, dulces y postres'                      WHERE CategoriaID = 3;
UPDATE dbo.Categorias SET NombreCategoria = N'Lácteos',            Descripcion = N'Quesos y otros productos lácteos'                  WHERE CategoriaID = 4;
UPDATE dbo.Categorias SET NombreCategoria = N'Carnes y Embutidos', Descripcion = N'Carnes preparadas y embutidos'                     WHERE CategoriaID = 5;
GO

-- Actualizar Proveedores
UPDATE dbo.Proveedores SET
    CompaniaNombre  = N'Lácteos García S.A.',
    NombreContacto  = N'Ana García',
    CargoContacto   = N'Gerente de Ventas',
    Direccion       = N'Av. Los Álamos 245',
    Ciudad          = N'Lima',
    CodigoPostal    = N'15024',
    Pais            = N'Perú',
    Telefono        = N'511-4567890',
    Fax             = N'511-4567891'
WHERE ProveedorID = 1;

UPDATE dbo.Proveedores SET
    CompaniaNombre  = N'Bebidas del Sur Ltda.',
    NombreContacto  = N'Carlos Ramírez',
    CargoContacto   = N'Jefe Comercial',
    Direccion       = N'Jr. Comercio 890',
    Ciudad          = N'Arequipa',
    CodigoPostal    = N'04001',
    Pais            = N'Perú',
    Telefono        = N'054-223344',
    Fax             = N'054-223345'
WHERE ProveedorID = 2;

UPDATE dbo.Proveedores SET
    CompaniaNombre  = N'Embutidos La Preferida',
    NombreContacto  = N'María Torres',
    CargoContacto   = N'Coordinadora de Distribución',
    Direccion       = N'Calle Las Flores 120',
    Ciudad          = N'Trujillo',
    CodigoPostal    = N'13001',
    Pais            = N'Perú',
    Telefono        = N'044-556677',
    Fax             = N'044-556678'
WHERE ProveedorID = 3;

UPDATE dbo.Proveedores SET
    CompaniaNombre  = N'Condimentos Andinos SAC',
    NombreContacto  = N'Jorge Quispe',
    CargoContacto   = N'Gerente General',
    Direccion       = N'Av. Industrial 500',
    Ciudad          = N'Cusco',
    CodigoPostal    = N'08001',
    Pais            = N'Perú',
    Telefono        = N'084-778899',
    Fax             = N'084-778900'
WHERE ProveedorID = 4;

UPDATE dbo.Proveedores SET
    CompaniaNombre  = N'Dulces del Valle E.I.R.L.',
    NombreContacto  = N'Lucía Fernández',
    CargoContacto   = N'Encargada de Ventas',
    Direccion       = N'Jr. San Martín 77',
    Ciudad          = N'Chiclayo',
    CodigoPostal    = N'14001',
    Pais            = N'Perú',
    Telefono        = N'074-991122',
    Fax             = N'074-991123'
WHERE ProveedorID = 5;
GO

-- Actualizar Clientes
UPDATE dbo.Clientes SET Empresa = N'Comercial Andina SAC',      NombreContacto = N'Pedro Salazar',         Ciudad = N'Lima',     Pais = N'Perú', Telefono = N'511-2345678' WHERE ClienteID = 1;
UPDATE dbo.Clientes SET Empresa = N'Supermercados del Norte',   NombreContacto = N'Rosa Medina',           Ciudad = N'Trujillo', Pais = N'Perú', Telefono = N'044-334455' WHERE ClienteID = 2;
UPDATE dbo.Clientes SET Empresa = N'Distribuidora Sureña EIRL', NombreContacto = N'Luis Chávez',           Ciudad = N'Arequipa', Pais = N'Perú', Telefono = N'054-667788' WHERE ClienteID = 3;
UPDATE dbo.Clientes SET Empresa = N'Minimarket Central',        NombreContacto = N'Elena Rojas',           Ciudad = N'Cusco',    Pais = N'Perú', Telefono = N'084-112233' WHERE ClienteID = 4;
UPDATE dbo.Clientes SET Empresa = N'Tiendas Express SAC',       NombreContacto = N'Miguel Ángel Paredes',  Ciudad = N'Chiclayo', Pais = N'Perú', Telefono = N'074-445566' WHERE ClienteID = 5;
GO

-- Actualizar Empleados
UPDATE dbo.Empleados SET Nombre = N'Juan',   Apellidos = N'Pérez Gómez',    Cargo = N'Vendedor',              Ciudad = N'Lima',     Pais = N'Perú' WHERE EmpleadoID = 1;
UPDATE dbo.Empleados SET Nombre = N'María',  Apellidos = N'López Díaz',     Cargo = N'Supervisora de Ventas', Ciudad = N'Lima',     Pais = N'Perú' WHERE EmpleadoID = 2;
UPDATE dbo.Empleados SET Nombre = N'Carlos', Apellidos = N'Ruiz Mendoza',   Cargo = N'Vendedor',              Ciudad = N'Arequipa', Pais = N'Perú' WHERE EmpleadoID = 3;
UPDATE dbo.Empleados SET Nombre = N'Sofía',  Apellidos = N'Vargas Castro',  Cargo = N'Gerente Regional',      Ciudad = N'Trujillo', Pais = N'Perú' WHERE EmpleadoID = 4;
UPDATE dbo.Empleados SET Nombre = N'Diego',  Apellidos = N'Fernández Ríos', Cargo = N'Vendedor',              Ciudad = N'Cusco',    Pais = N'Perú' WHERE EmpleadoID = 5;
GO

-- Actualizar Transportistas
UPDATE dbo.Transportistas SET CompaniaNombre = N'Transportes Rápido SAC', Telefono = N'511-8889900' WHERE TransportistaID = 1;
UPDATE dbo.Transportistas SET CompaniaNombre = N'Envíos Seguros EIRL',    Telefono = N'511-7776655' WHERE TransportistaID = 2;
UPDATE dbo.Transportistas SET CompaniaNombre = N'Logística del Pacífico',  Telefono = N'054-990011' WHERE TransportistaID = 3;
UPDATE dbo.Transportistas SET CompaniaNombre = N'Courier Nacional SA',     Telefono = N'044-223344' WHERE TransportistaID = 4;
UPDATE dbo.Transportistas SET CompaniaNombre = N'TransAndino Express',     Telefono = N'084-556677' WHERE TransportistaID = 5;
GO

-- Actualizar Productos
UPDATE dbo.Productos SET NombreProducto = N'Café Andino Premium'     WHERE ProductoID = 1;
UPDATE dbo.Productos SET NombreProducto = N'Salsa de Ají Amarillo'   WHERE ProductoID = 2;
UPDATE dbo.Productos SET NombreProducto = N'Mermelada de Aguaymanto' WHERE ProductoID = 3;
UPDATE dbo.Productos SET NombreProducto = N'Queso Fresco Andino'     WHERE ProductoID = 4;
UPDATE dbo.Productos SET NombreProducto = N'Chorizo Ahumado'         WHERE ProductoID = 5;
GO

-- Actualizar Pedidos (destinatarios y ciudades)
UPDATE dbo.Pedidos SET Destinatario = N'Comercial Andina SAC',      CiudadDestino = N'Lima',     PaisDestino = N'Perú' WHERE PedidoID = 1;
UPDATE dbo.Pedidos SET Destinatario = N'Supermercados del Norte',   CiudadDestino = N'Trujillo', PaisDestino = N'Perú' WHERE PedidoID = 2;
UPDATE dbo.Pedidos SET Destinatario = N'Distribuidora Sureña EIRL', CiudadDestino = N'Arequipa', PaisDestino = N'Perú' WHERE PedidoID = 3;
UPDATE dbo.Pedidos SET Destinatario = N'Minimarket Central',        CiudadDestino = N'Cusco',    PaisDestino = N'Perú' WHERE PedidoID = 4;
UPDATE dbo.Pedidos SET Destinatario = N'Tiendas Express SAC',       CiudadDestino = N'Chiclayo', PaisDestino = N'Perú' WHERE PedidoID = 5;
GO

-- Verificar
SELECT CompaniaNombre, NombreContacto, Ciudad FROM dbo.Proveedores;
SELECT Nombre, Apellidos FROM dbo.Empleados;
GO
