using WPF_SP.Models;

namespace WPF_SP.Data;

public interface IProductoRepository
{
    Task<List<Producto>> ListarAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Producto p);
    Task ActualizarAsync(Producto p);
    Task EliminarAsync(int id);
}
