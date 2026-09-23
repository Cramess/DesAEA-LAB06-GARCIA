using WPF_SP.Data.Models;

namespace WPF_SP.Data.Repositories;

public interface ICategoriaRepository
{
    Task<List<Categoria>> ListarAsync();
    Task<Categoria?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(string nombre, string? descripcion);
    Task ActualizarAsync(int id, string nombre, string? descripcion);
    Task EliminarAsync(int id);
}
