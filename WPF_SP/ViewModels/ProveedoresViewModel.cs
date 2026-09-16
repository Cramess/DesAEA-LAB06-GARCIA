using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_SP.Data;
using WPF_SP.Models;

namespace WPF_SP.ViewModels;

public partial class ProveedoresViewModel : ObservableObject
{
    private readonly IProveedorRepository _repo;

    public ObservableCollection<Proveedor> Proveedores { get; } = new();

    [ObservableProperty] private Proveedor? proveedorSeleccionado;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? filtroBusquedaCompania;
    [ObservableProperty] private string? filtroBusquedaContacto;
    [ObservableProperty] private string? filtroBusquedaCiudad;
    [ObservableProperty] private string companiaNombre = string.Empty;
    [ObservableProperty] private string? nombreContacto;
    [ObservableProperty] private string? cargoContacto;
    [ObservableProperty] private string? direccion;
    [ObservableProperty] private string? ciudad;
    [ObservableProperty] private string? codigoPostal;
    [ObservableProperty] private string? pais;
    [ObservableProperty] private string? telefono;
    [ObservableProperty] private string? fax;

    private bool _isEditing;
    private int _editingId;

    public ProveedoresViewModel(IProveedorRepository repo) => _repo = repo;

    [RelayCommand]
    public async Task CargarAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var items = await _repo.ListarAsync();
            Proveedores.Clear();
            foreach (var p in items) Proveedores.Add(p);
        }
        catch (Exception ex) { ErrorMessage = $"Error al cargar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task BuscarAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var comp     = string.IsNullOrWhiteSpace(FiltroBusquedaCompania) ? null : FiltroBusquedaCompania.Trim();
            var contacto = string.IsNullOrWhiteSpace(FiltroBusquedaContacto) ? null : FiltroBusquedaContacto.Trim();
            var ciu      = string.IsNullOrWhiteSpace(FiltroBusquedaCiudad)   ? null : FiltroBusquedaCiudad.Trim();
            var items    = await _repo.BuscarAsync(contacto, ciu, comp);
            Proveedores.Clear();
            foreach (var p in items) Proveedores.Add(p);
        }
        catch (Exception ex) { ErrorMessage = $"Error en búsqueda: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task LimpiarFiltrosAsync()
    {
        FiltroBusquedaCompania = null;
        FiltroBusquedaContacto = null;
        FiltroBusquedaCiudad   = null;
        await CargarAsync();
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(CompaniaNombre))
        {
            ErrorMessage = "El nombre de la compañía es obligatorio.";
            return;
        }
        ErrorMessage = null;
        try
        {
            var p = BuildProveedor();
            if (_isEditing) { p.ProveedorID = _editingId; await _repo.ActualizarAsync(p); }
            else             await _repo.CrearAsync(p);
            LimpiarFormulario();
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al guardar: {ex.Message}"; }
    }

    [RelayCommand]
    private void EditarSeleccionado()
    {
        if (ProveedorSeleccionado is null) return;
        _isEditing     = true;
        _editingId     = ProveedorSeleccionado.ProveedorID;
        CompaniaNombre = ProveedorSeleccionado.CompaniaNombre;
        NombreContacto = ProveedorSeleccionado.NombreContacto;
        CargoContacto  = ProveedorSeleccionado.CargoContacto;
        Direccion      = ProveedorSeleccionado.Direccion;
        Ciudad         = ProveedorSeleccionado.Ciudad;
        CodigoPostal   = ProveedorSeleccionado.CodigoPostal;
        Pais           = ProveedorSeleccionado.Pais;
        Telefono       = ProveedorSeleccionado.Telefono;
        Fax            = ProveedorSeleccionado.Fax;
    }

    [RelayCommand]
    private async Task EliminarSeleccionadoAsync()
    {
        if (ProveedorSeleccionado is null) return;
        var res = MessageBox.Show($"¿Dar de baja lógica al proveedor \"{ProveedorSeleccionado.CompaniaNombre}\"?",
            "Confirmar Baja Lógica", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (res != MessageBoxResult.Yes) return;
        ErrorMessage = null;
        try
        {
            await _repo.EliminarAsync(ProveedorSeleccionado.ProveedorID);
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al dar de baja lógica: {ex.Message}"; }
    }

    [RelayCommand]
    private void CancelarEdicion() => LimpiarFormulario();

    private Proveedor BuildProveedor() => new()
    {
        CompaniaNombre = CompaniaNombre.Trim(),
        NombreContacto = NombreContacto?.Trim(),
        CargoContacto  = CargoContacto?.Trim(),
        Direccion      = Direccion?.Trim(),
        Ciudad         = Ciudad?.Trim(),
        CodigoPostal   = CodigoPostal?.Trim(),
        Pais           = Pais?.Trim(),
        Telefono       = Telefono?.Trim(),
        Fax            = Fax?.Trim()
    };

    private void LimpiarFormulario()
    {
        _isEditing     = false;
        _editingId     = 0;
        CompaniaNombre = string.Empty;
        NombreContacto = null;
        CargoContacto  = null;
        Direccion      = null;
        Ciudad         = null;
        CodigoPostal   = null;
        Pais           = null;
        Telefono       = null;
        Fax            = null;
        ProveedorSeleccionado = null;
    }
}
