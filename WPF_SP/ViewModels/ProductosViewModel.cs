using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_SP.Data;
using WPF_SP.Models;

namespace WPF_SP.ViewModels;

public partial class ProductosViewModel : ObservableObject
{
    private readonly IProductoRepository  _repoProd;
    private readonly ICategoriaRepository _repoCat;
    private readonly IProveedorRepository _repoProv;

    public ObservableCollection<Producto>   Productos   { get; } = new();
    public ObservableCollection<Categoria>  Categorias  { get; } = new();
    public ObservableCollection<Proveedor>  Proveedores { get; } = new();

    [ObservableProperty] private Producto? productoSeleccionado;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string nombreProducto = string.Empty;
    [ObservableProperty] private Categoria? categoriaSeleccionada;
    [ObservableProperty] private Proveedor? proveedorSeleccionado;
    [ObservableProperty] private string? cantidadPorUnidad;
    [ObservableProperty] private decimal precioUnidad;
    [ObservableProperty] private short unidadesEnExistencia;
    [ObservableProperty] private short unidadesEnPedido;
    [ObservableProperty] private short nivelDeReorden;
    [ObservableProperty] private bool descontinuado;

    private bool _isEditing;
    private int _editingId;

    public ProductosViewModel(IProductoRepository repoProd, ICategoriaRepository repoCat, IProveedorRepository repoProv)
    {
        _repoProd = repoProd;
        _repoCat  = repoCat;
        _repoProv = repoProv;
    }

    [RelayCommand]
    public async Task CargarAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var cats  = await _repoCat.ListarAsync();
            var provs = await _repoProv.ListarAsync();
            var prods = await _repoProd.ListarAsync();

            Categorias.Clear();
            foreach (var c in cats) Categorias.Add(c);

            Proveedores.Clear();
            foreach (var p in provs) Proveedores.Add(p);

            Productos.Clear();
            foreach (var p in prods) Productos.Add(p);
        }
        catch (Exception ex) { ErrorMessage = $"Error al cargar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreProducto))
        {
            ErrorMessage = "El nombre es obligatorio.";
            return;
        }
        ErrorMessage = null;
        try
        {
            var p = BuildProducto();
            if (_isEditing) { p.ProductoID = _editingId; await _repoProd.ActualizarAsync(p); }
            else             await _repoProd.CrearAsync(p);
            LimpiarFormulario();
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al guardar: {ex.Message}"; }
    }

    [RelayCommand]
    private void EditarSeleccionado()
    {
        if (ProductoSeleccionado is null) return;
        _isEditing    = true;
        _editingId    = ProductoSeleccionado.ProductoID;
        NombreProducto    = ProductoSeleccionado.NombreProducto;
        CantidadPorUnidad = ProductoSeleccionado.CantidadPorUnidad;
        PrecioUnidad          = ProductoSeleccionado.PrecioUnidad;
        UnidadesEnExistencia  = ProductoSeleccionado.UnidadesEnExistencia;
        UnidadesEnPedido      = ProductoSeleccionado.UnidadesEnPedido;
        NivelDeReorden        = ProductoSeleccionado.NivelDeReorden;
        Descontinuado         = ProductoSeleccionado.Descontinuado;
        CategoriaSeleccionada = Categorias.FirstOrDefault(c => c.CategoriaID == ProductoSeleccionado.CategoriaID);
        ProveedorSeleccionado = Proveedores.FirstOrDefault(p => p.ProveedorID == ProductoSeleccionado.ProveedorID);
    }

    [RelayCommand]
    private async Task EliminarSeleccionadoAsync()
    {
        if (ProductoSeleccionado is null) return;
        var res = MessageBox.Show($"¿Eliminar el producto \"{ProductoSeleccionado.NombreProducto}\"?",
            "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (res != MessageBoxResult.Yes) return;
        ErrorMessage = null;
        try
        {
            await _repoProd.EliminarAsync(ProductoSeleccionado.ProductoID);
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al eliminar: {ex.Message}"; }
    }

    [RelayCommand]
    private void CancelarEdicion() => LimpiarFormulario();

    private Producto BuildProducto() => new()
    {
        NombreProducto       = NombreProducto.Trim(),
        CategoriaID          = CategoriaSeleccionada?.CategoriaID,
        ProveedorID          = ProveedorSeleccionado?.ProveedorID,
        CantidadPorUnidad    = CantidadPorUnidad?.Trim(),
        PrecioUnidad         = PrecioUnidad,
        UnidadesEnExistencia = UnidadesEnExistencia,
        UnidadesEnPedido     = UnidadesEnPedido,
        NivelDeReorden       = NivelDeReorden,
        Descontinuado        = Descontinuado
    };

    private void LimpiarFormulario()
    {
        _isEditing            = false;
        _editingId            = 0;
        NombreProducto        = string.Empty;
        CantidadPorUnidad     = null;
        PrecioUnidad          = 0;
        UnidadesEnExistencia  = 0;
        UnidadesEnPedido      = 0;
        NivelDeReorden        = 0;
        Descontinuado         = false;
        CategoriaSeleccionada = null;
        ProveedorSeleccionado = null;
        ProductoSeleccionado  = null;
    }
}
