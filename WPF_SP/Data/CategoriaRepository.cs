using System.Data;
using Microsoft.Data.SqlClient;
using WPF_SP.Models;

namespace WPF_SP.Data;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly string _cs;
    public CategoriaRepository(string connectionString) => _cs = connectionString;

    public async Task<List<Categoria>> ListarAsync()
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Categoria_Listar", con) { CommandType = CommandType.StoredProcedure };
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        var list = new List<Categoria>();
        while (await r.ReadAsync()) list.Add(Map(r));
        return list;
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Categoria_ObtenerPorId", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@CategoriaID", SqlDbType.Int).Value = id;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        return await r.ReadAsync() ? Map(r) : null;
    }

    public async Task<int> CrearAsync(string nombre, string? descripcion)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Categoria_Crear", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@NombreCategoria", SqlDbType.NVarChar, 30).Value = nombre;
        cmd.Parameters.Add("@Descripcion",     SqlDbType.NVarChar, 200).Value = (object?)descripcion ?? DBNull.Value;
        var paramNuevoId = cmd.Parameters.Add("@NuevoID", SqlDbType.Int);
        paramNuevoId.Direction = ParameterDirection.Output;

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        return paramNuevoId.Value != DBNull.Value ? Convert.ToInt32(paramNuevoId.Value) : 0;
    }

    public async Task ActualizarAsync(int id, string nombre, string? descripcion)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Categoria_Actualizar", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@CategoriaID",     SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@NombreCategoria", SqlDbType.NVarChar, 30).Value = nombre;
        cmd.Parameters.Add("@Descripcion",     SqlDbType.NVarChar, 200).Value = (object?)descripcion ?? DBNull.Value;

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int id)
    {
        await using var con = new SqlConnection(_cs);
        await using var cmd = new SqlCommand("dbo.usp_Categoria_EliminarLogico", con) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.Add("@CategoriaID", SqlDbType.Int).Value = id;

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static Categoria Map(SqlDataReader r) => new()
    {
        CategoriaID     = r.GetInt32(r.GetOrdinal("CategoriaID")),
        NombreCategoria = r.GetString(r.GetOrdinal("NombreCategoria")),
        Descripcion     = r.IsDBNull(r.GetOrdinal("Descripcion")) ? null : r.GetString(r.GetOrdinal("Descripcion")),
        Activo          = !r.IsDBNull(r.GetOrdinal("Activo")) && r.GetBoolean(r.GetOrdinal("Activo"))
    };
}
