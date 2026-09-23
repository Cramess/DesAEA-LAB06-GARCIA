using WPF_SP.Data.Models;

namespace WPF_SP.Data.Repositories;

public interface IProveedorRepository
{
    Task<List<Proveedor>> ListarAsync();
    Task<List<Proveedor>> BuscarAsync(string? nombreContacto, string? ciudad, string? companiaNombre = null);
    Task<Proveedor?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Proveedor p);
    Task ActualizarAsync(Proveedor p);
    Task EliminarAsync(int id);
}
