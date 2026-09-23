using System.Data;
using Microsoft.Data.SqlClient;
using WPF_SP.Data.Models;

namespace WPF_SP.Data.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly string _cs;
    public ProveedorRepository(string connectionString) => _cs = connectionString;

    public async Task<List<Proveedor>> ListarAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Proveedor>();
        while (await r.ReadAsync()) list.Add(Map(r));
        return list;
    }

    public async Task<List<Proveedor>> BuscarAsync(string? nombreContacto, string? ciudad, string? companiaNombre = null)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_Buscar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@NombreContacto", SqlDbType.NVarChar, 40).Value = (object?)nombreContacto ?? DBNull.Value;
        cmd.Parameters.Add("@Ciudad",         SqlDbType.NVarChar, 30).Value = (object?)ciudad         ?? DBNull.Value;
        cmd.Parameters.Add("@CompaniaNombre", SqlDbType.NVarChar, 60).Value = (object?)companiaNombre ?? DBNull.Value;

        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Proveedor>();
        while (await r.ReadAsync()) list.Add(Map(r));
        return list;
    }

    public async Task<Proveedor?> ObtenerPorIdAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_ObtenerPorId", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? Map(r) : null;
    }

    public async Task<int> CrearAsync(Proveedor p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_Crear", con) { CommandType = CommandType.StoredProcedure };
        AddParams(cmd, p);
        var paramNuevoId = cmd.Parameters.Add("@NuevoID", SqlDbType.Int);
        paramNuevoId.Direction = ParameterDirection.Output;

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        var nuevoId = paramNuevoId.Value != DBNull.Value ? Convert.ToInt32(paramNuevoId.Value) : 0;
        p.ProveedorID = nuevoId;
        return nuevoId;
    }

    public async Task ActualizarAsync(Proveedor p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_Actualizar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = p.ProveedorID;
        AddParams(cmd, p);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Proveedor_EliminarLogico", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProveedorID", SqlDbType.Int).Value = id;

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static void AddParams(SqlCommand cmd, Proveedor p)
    {
        cmd.Parameters.Add("@CompaniaNombre", SqlDbType.NVarChar, 60).Value = p.CompaniaNombre;
        cmd.Parameters.Add("@NombreContacto", SqlDbType.NVarChar, 40).Value = (object?)p.NombreContacto ?? DBNull.Value;
        cmd.Parameters.Add("@CargoContacto",  SqlDbType.NVarChar, 40).Value = (object?)p.CargoContacto  ?? DBNull.Value;
        cmd.Parameters.Add("@Direccion",      SqlDbType.NVarChar, 80).Value = (object?)p.Direccion      ?? DBNull.Value;
        cmd.Parameters.Add("@Ciudad",         SqlDbType.NVarChar, 30).Value = (object?)p.Ciudad         ?? DBNull.Value;
        cmd.Parameters.Add("@CodigoPostal",   SqlDbType.NVarChar, 10).Value = (object?)p.CodigoPostal   ?? DBNull.Value;
        cmd.Parameters.Add("@Pais",           SqlDbType.NVarChar, 30).Value = (object?)p.Pais           ?? DBNull.Value;
        cmd.Parameters.Add("@Telefono",       SqlDbType.NVarChar, 24).Value = (object?)p.Telefono       ?? DBNull.Value;
        cmd.Parameters.Add("@Fax",            SqlDbType.NVarChar, 24).Value = (object?)p.Fax            ?? DBNull.Value;
    }

    private static string? Str(SqlDataReader r, string col) =>
        r.IsDBNull(r.GetOrdinal(col)) ? null : r.GetString(r.GetOrdinal(col));

    private static Proveedor Map(SqlDataReader r) => new()
    {
        ProveedorID    = r.GetInt32(r.GetOrdinal("ProveedorID")),
        CompaniaNombre = r.GetString(r.GetOrdinal("CompaniaNombre")),
        NombreContacto = Str(r, "NombreContacto"),
        CargoContacto  = Str(r, "CargoContacto"),
        Direccion      = Str(r, "Direccion"),
        Ciudad         = Str(r, "Ciudad"),
        CodigoPostal   = Str(r, "CodigoPostal"),
        Pais           = Str(r, "Pais"),
        Telefono       = Str(r, "Telefono"),
        Fax            = Str(r, "Fax"),
        Activo         = !r.IsDBNull(r.GetOrdinal("Activo")) && r.GetBoolean(r.GetOrdinal("Activo"))
    };
}
