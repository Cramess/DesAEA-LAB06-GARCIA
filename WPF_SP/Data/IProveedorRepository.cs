using WPF_SP.Models;

namespace WPF_SP.Data;

public interface IProveedorRepository
{
    Task<List<Proveedor>> ListarAsync();
    Task<List<Proveedor>> BuscarAsync(string? nombreContacto, string? ciudad);
    Task<Proveedor?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Proveedor p);
    Task ActualizarAsync(Proveedor p);
    Task EliminarAsync(int id);
}
