using System.Data;
using Microsoft.Data.SqlClient;
using WPF_SP.Models;

namespace WPF_SP.Data;

public class ProductoRepository : IProductoRepository
{
    private readonly string _cs;
    public ProductoRepository(string connectionString) => _cs = connectionString;

    public async Task<List<Producto>> ListarAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Producto_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Producto>();
        while (await r.ReadAsync()) list.Add(Map(r));
        return list;
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Producto_ObtenerPorId", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? Map(r) : null;
    }

    public async Task<int> CrearAsync(Producto p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Producto_Crear", con) { CommandType = CommandType.StoredProcedure };
        AddParams(cmd, p);
        await con.OpenAsync();
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task ActualizarAsync(Producto p)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Producto_Actualizar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = p.ProductoID;
        AddParams(cmd, p);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Producto_Eliminar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static void AddParams(SqlCommand cmd, Producto p)
    {
        cmd.Parameters.Add("@NombreProducto",      SqlDbType.NVarChar, 60).Value   = p.NombreProducto;
        cmd.Parameters.Add("@ProveedorID",          SqlDbType.Int).Value            = (object?)p.ProveedorID ?? DBNull.Value;
        cmd.Parameters.Add("@CategoriaID",          SqlDbType.Int).Value            = (object?)p.CategoriaID ?? DBNull.Value;
        cmd.Parameters.Add("@CantidadPorUnidad",    SqlDbType.NVarChar, 30).Value  = (object?)p.CantidadPorUnidad ?? DBNull.Value;
        cmd.Parameters.Add("@PrecioUnidad",         SqlDbType.Decimal).Value        = p.PrecioUnidad;
        cmd.Parameters.Add("@UnidadesEnExistencia", SqlDbType.SmallInt).Value       = p.UnidadesEnExistencia;
        cmd.Parameters.Add("@UnidadesEnPedido",     SqlDbType.SmallInt).Value       = p.UnidadesEnPedido;
        cmd.Parameters.Add("@NivelDeReorden",       SqlDbType.SmallInt).Value       = p.NivelDeReorden;
        cmd.Parameters.Add("@Descontinuado",        SqlDbType.Bit).Value            = p.Descontinuado;
    }

    private static string? Str(SqlDataReader r, string col) =>
        r.IsDBNull(r.GetOrdinal(col)) ? null : r.GetString(r.GetOrdinal(col));

    private static Producto Map(SqlDataReader r) => new()
    {
        ProductoID           = r.GetInt32(r.GetOrdinal("ProductoID")),
        NombreProducto       = r.GetString(r.GetOrdinal("NombreProducto")),
        ProveedorID          = r.IsDBNull(r.GetOrdinal("ProveedorID"))  ? null : r.GetInt32(r.GetOrdinal("ProveedorID")),
        CategoriaID          = r.IsDBNull(r.GetOrdinal("CategoriaID"))  ? null : r.GetInt32(r.GetOrdinal("CategoriaID")),
        CantidadPorUnidad    = Str(r, "CantidadPorUnidad"),
        PrecioUnidad         = r.GetDecimal(r.GetOrdinal("PrecioUnidad")),
        UnidadesEnExistencia = r.GetInt16(r.GetOrdinal("UnidadesEnExistencia")),
        UnidadesEnPedido     = r.GetInt16(r.GetOrdinal("UnidadesEnPedido")),
        NivelDeReorden       = r.GetInt16(r.GetOrdinal("NivelDeReorden")),
        Descontinuado        = r.GetBoolean(r.GetOrdinal("Descontinuado")),
        NombreCategoria      = Str(r, "NombreCategoria"),
        NombreProveedor      = Str(r, "NombreProveedor")
    };
}
