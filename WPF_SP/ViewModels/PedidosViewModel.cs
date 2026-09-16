using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_SP.Data;
using WPF_SP.Models;

namespace WPF_SP.ViewModels;

public partial class PedidosViewModel : ObservableObject
{
    private readonly IPedidoRepository _repo;

    public ObservableCollection<Pedido>        Pedidos        { get; } = new();
    public ObservableCollection<Cliente>       Clientes       { get; } = new();
    public ObservableCollection<Empleado>      Empleados      { get; } = new();
    public ObservableCollection<Transportista> Transportistas { get; } = new();
    [ObservableProperty] private Pedido? pedidoSeleccionado;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private Cliente? clienteSeleccionado;
    [ObservableProperty] private Empleado? empleadoSeleccionado;
    [ObservableProperty] private Transportista? transportistaSeleccionada;
    [ObservableProperty] private DateTime fechaPedido = DateTime.Today;
    [ObservableProperty] private DateTime? fechaRequerida;
    [ObservableProperty] private DateTime? fechaEnvio;
    [ObservableProperty] private string? destinatario;
    [ObservableProperty] private string? ciudadDestino;
    [ObservableProperty] private string? paisDestino;

    private bool _isEditing;
    private int _editingId;

    public PedidosViewModel(IPedidoRepository repo) => _repo = repo;

    [RelayCommand]
    public async Task CargarAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var clientes       = await _repo.ListarClientesAsync();
            var empleados      = await _repo.ListarEmpleadosAsync();
            var transportistas = await _repo.ListarTransportistasAsync();
            var pedidos        = await _repo.ListarAsync();

            Clientes.Clear();       foreach (var c in clientes)       Clientes.Add(c);
            Empleados.Clear();      foreach (var e in empleados)       Empleados.Add(e);
            Transportistas.Clear(); foreach (var t in transportistas)  Transportistas.Add(t);
            Pedidos.Clear();        foreach (var p in pedidos)         Pedidos.Add(p);
        }
        catch (Exception ex) { ErrorMessage = $"Error al cargar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        ErrorMessage = null;
        try
        {
            var p = BuildPedido();
            if (_isEditing) { p.PedidoID = _editingId; await _repo.ActualizarAsync(p); }
            else             await _repo.CrearAsync(p);
            LimpiarFormulario();
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al guardar: {ex.Message}"; }
    }

    [RelayCommand]
    private void EditarSeleccionado()
    {
        if (PedidoSeleccionado is null) return;
        _isEditing              = true;
        _editingId              = PedidoSeleccionado.PedidoID;
        FechaPedido             = PedidoSeleccionado.FechaPedido;
        FechaRequerida          = PedidoSeleccionado.FechaRequerida;
        FechaEnvio              = PedidoSeleccionado.FechaEnvio;
        Destinatario            = PedidoSeleccionado.Destinatario;
        CiudadDestino           = PedidoSeleccionado.CiudadDestino;
        PaisDestino             = PedidoSeleccionado.PaisDestino;
        ClienteSeleccionado     = Clientes.FirstOrDefault(c => c.ClienteID       == PedidoSeleccionado.ClienteID);
        EmpleadoSeleccionado    = Empleados.FirstOrDefault(e => e.EmpleadoID     == PedidoSeleccionado.EmpleadoID);
        TransportistaSeleccionada = Transportistas.FirstOrDefault(t => t.TransportistaID == PedidoSeleccionado.TransportistaID);
    }

    [RelayCommand]
    private async Task EliminarSeleccionadoAsync()
    {
        if (PedidoSeleccionado is null) return;
        var res = MessageBox.Show($"¿Dar de baja lógica al pedido #{PedidoSeleccionado.PedidoID}?",
            "Confirmar Baja Lógica", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (res != MessageBoxResult.Yes) return;
        ErrorMessage = null;
        try
        {
            await _repo.EliminarAsync(PedidoSeleccionado.PedidoID);
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al dar de baja lógica: {ex.Message}"; }
    }

    [RelayCommand]
    private void CancelarEdicion() => LimpiarFormulario();

    private Pedido BuildPedido() => new()
    {
        ClienteID        = ClienteSeleccionado?.ClienteID,
        EmpleadoID       = EmpleadoSeleccionado?.EmpleadoID,
        FechaPedido      = FechaPedido,
        FechaRequerida   = FechaRequerida,
        FechaEnvio       = FechaEnvio,
        TransportistaID  = TransportistaSeleccionada?.TransportistaID,
        Destinatario     = Destinatario?.Trim(),
        CiudadDestino    = CiudadDestino?.Trim(),
        PaisDestino      = PaisDestino?.Trim()
    };

    private void LimpiarFormulario()
    {
        _isEditing                = false;
        _editingId                = 0;
        FechaPedido               = DateTime.Today;
        FechaRequerida            = null;
        FechaEnvio                = null;
        Destinatario              = null;
        CiudadDestino             = null;
        PaisDestino               = null;
        ClienteSeleccionado       = null;
        EmpleadoSeleccionado      = null;
        TransportistaSeleccionada = null;
        PedidoSeleccionado        = null;
    }
}
