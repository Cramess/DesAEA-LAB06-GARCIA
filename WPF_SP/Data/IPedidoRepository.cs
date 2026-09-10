using WPF_SP.Models;

namespace WPF_SP.Data;

public interface IPedidoRepository
{
    Task<List<Pedido>> ListarAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(Pedido p);
    Task ActualizarAsync(Pedido p);
    Task EliminarAsync(int id);
    Task<List<DetallePedidoReporte>> ObtenerReporteAsync(DateTime inicio, DateTime fin);
    Task<List<Cliente>> ListarClientesAsync();
    Task<List<Empleado>> ListarEmpleadosAsync();
    Task<List<Transportista>> ListarTransportistasAsync();
}
