using System.Data;
using Microsoft.Data.SqlClient;
using WPF_SP.Models;

namespace WPF_SP.Data;

public class PedidoRepository : IPedidoRepository
{
    private readonly string _cs;
    public PedidoRepository(string connectionString) => _cs = connectionString;

    public async Task<List<Pedido>> ListarAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Pedido_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Pedido>();
        while (await r.ReadAsync()) list.Add(MapPedido(r));
        return list;
    }

    public async Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Pedido_ObtenerPorId", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@PedidoID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? MapPedido(r) : null;
    }

    public async Task<int> CrearAsync(Pedido p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Pedido_Crear", con) { CommandType = CommandType.StoredProcedure };
        AddParams(cmd, p);
        await con.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task ActualizarAsync(Pedido p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Pedido_Actualizar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@PedidoID", SqlDbType.Int).Value = p.PedidoID;
        AddParams(cmd, p);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Pedido_Eliminar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@PedidoID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<DetallePedidoReporte>> ObtenerReporteAsync(DateTime inicio, DateTime fin)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_DetallePedidos_ReportePorFechas", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = inicio.Date;
        cmd.Parameters.Add("@FechaFin",    SqlDbType.Date).Value = fin.Date;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<DetallePedidoReporte>();
        while (await r.ReadAsync()) list.Add(MapDetalle(r));
        return list;
    }

    public async Task<List<Cliente>> ListarClientesAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Cliente_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Cliente>();
        while (await r.ReadAsync())
            list.Add(new Cliente
            {
                ClienteID      = r.GetInt32(r.GetOrdinal("ClienteID")),
                Empresa        = r.GetString(r.GetOrdinal("Empresa")),
                NombreContacto = r.IsDBNull(r.GetOrdinal("NombreContacto")) ? null : r.GetString(r.GetOrdinal("NombreContacto")),
                Ciudad         = r.IsDBNull(r.GetOrdinal("Ciudad"))         ? null : r.GetString(r.GetOrdinal("Ciudad")),
                Pais           = r.IsDBNull(r.GetOrdinal("Pais"))           ? null : r.GetString(r.GetOrdinal("Pais")),
                Telefono       = r.IsDBNull(r.GetOrdinal("Telefono"))       ? null : r.GetString(r.GetOrdinal("Telefono"))
            });
        return list;
    }

    public async Task<List<Empleado>> ListarEmpleadosAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Empleado_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Empleado>();
        while (await r.ReadAsync())
            list.Add(new Empleado
            {
                EmpleadoID = r.GetInt32(r.GetOrdinal("EmpleadoID")),
                Nombre     = r.GetString(r.GetOrdinal("Nombre")),
                Apellidos  = r.GetString(r.GetOrdinal("Apellidos")),
                Cargo      = r.IsDBNull(r.GetOrdinal("Cargo")) ? null : r.GetString(r.GetOrdinal("Cargo"))
            });
        return list;
    }

    public async Task<List<Transportista>> ListarTransportistasAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Transportista_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Transportista>();
        while (await r.ReadAsync())
            list.Add(new Transportista
            {
                TransportistaID = r.GetInt32(r.GetOrdinal("TransportistaID")),
                CompaniaNombre  = r.GetString(r.GetOrdinal("CompaniaNombre")),
                Telefono        = r.IsDBNull(r.GetOrdinal("Telefono")) ? null : r.GetString(r.GetOrdinal("Telefono"))
            });
        return list;
    }

    private static void AddParams(SqlCommand cmd, Pedido p)
    {
        cmd.Parameters.Add("@ClienteID",       SqlDbType.Int).Value          = (object?)p.ClienteID       ?? DBNull.Value;
        cmd.Parameters.Add("@EmpleadoID",      SqlDbType.Int).Value          = (object?)p.EmpleadoID      ?? DBNull.Value;
        cmd.Parameters.Add("@FechaPedido",     SqlDbType.Date).Value         = p.FechaPedido;
        cmd.Parameters.Add("@FechaRequerida",  SqlDbType.Date).Value         = (object?)p.FechaRequerida  ?? DBNull.Value;
        cmd.Parameters.Add("@FechaEnvio",      SqlDbType.Date).Value         = (object?)p.FechaEnvio      ?? DBNull.Value;
        cmd.Parameters.Add("@TransportistaID", SqlDbType.Int).Value          = (object?)p.TransportistaID ?? DBNull.Value;
        cmd.Parameters.Add("@Destinatario",    SqlDbType.NVarChar, 60).Value = (object?)p.Destinatario    ?? DBNull.Value;
        cmd.Parameters.Add("@CiudadDestino",   SqlDbType.NVarChar, 30).Value = (object?)p.CiudadDestino   ?? DBNull.Value;
        cmd.Parameters.Add("@PaisDestino",     SqlDbType.NVarChar, 30).Value = (object?)p.PaisDestino     ?? DBNull.Value;
    }

    private static string? Str(SqlDataReader r, string col) =>
        r.IsDBNull(r.GetOrdinal(col)) ? null : r.GetString(r.GetOrdinal(col));

    private static DateTime? NullDate(SqlDataReader r, string col) =>
        r.IsDBNull(r.GetOrdinal(col)) ? null : r.GetDateTime(r.GetOrdinal(col));

    private static Pedido MapPedido(SqlDataReader r) => new()
    {
        PedidoID           = r.GetInt32(r.GetOrdinal("PedidoID")),
        ClienteID          = r.IsDBNull(r.GetOrdinal("ClienteID"))       ? null : r.GetInt32(r.GetOrdinal("ClienteID")),
        EmpleadoID         = r.IsDBNull(r.GetOrdinal("EmpleadoID"))      ? null : r.GetInt32(r.GetOrdinal("EmpleadoID")),
        FechaPedido        = r.GetDateTime(r.GetOrdinal("FechaPedido")),
        FechaRequerida     = NullDate(r, "FechaRequerida"),
        FechaEnvio         = NullDate(r, "FechaEnvio"),
        TransportistaID    = r.IsDBNull(r.GetOrdinal("TransportistaID")) ? null : r.GetInt32(r.GetOrdinal("TransportistaID")),
        Destinatario       = Str(r, "Destinatario"),
        CiudadDestino      = Str(r, "CiudadDestino"),
        PaisDestino        = Str(r, "PaisDestino"),
        NombreCliente      = Str(r, "NombreCliente"),
        NombreEmpleado     = Str(r, "NombreEmpleado"),
        NombreTransportista = Str(r, "NombreTransportista")
    };

    private static DetallePedidoReporte MapDetalle(SqlDataReader r) => new()
    {
        PedidoID       = r.GetInt32(r.GetOrdinal("PedidoID")),
        FechaPedido    = r.GetDateTime(r.GetOrdinal("FechaPedido")),
        FechaEnvio     = NullDate(r, "FechaEnvio"),
        Destinatario   = Str(r, "Destinatario"),
        CiudadDestino  = Str(r, "CiudadDestino"),
        PaisDestino    = Str(r, "PaisDestino"),
        NombreCliente  = Str(r, "NombreCliente"),
        NombreProducto = r.GetString(r.GetOrdinal("NombreProducto")),
        PrecioUnidad   = r.GetDecimal(r.GetOrdinal("PrecioUnidad")),
        Cantidad       = r.GetInt16(r.GetOrdinal("Cantidad")),
        Descuento      = r.GetDecimal(r.GetOrdinal("Descuento")),
        SubTotal       = r.GetDecimal(r.GetOrdinal("SubTotal"))
    };
}
