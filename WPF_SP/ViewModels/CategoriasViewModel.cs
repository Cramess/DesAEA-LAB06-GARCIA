using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_SP.Data;
using WPF_SP.Models;

namespace WPF_SP.ViewModels;

public partial class CategoriasViewModel : ObservableObject
{
    private readonly ICategoriaRepository _repo;

    public ObservableCollection<Categoria> Categorias { get; } = new();

    [ObservableProperty] private Categoria? categoriaSeleccionada;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string nombreCategoria = string.Empty;
    [ObservableProperty] private string? descripcion;

    private bool _isEditing;
    private int _editingId;

    public CategoriasViewModel(ICategoriaRepository repo) => _repo = repo;

    [RelayCommand]
    public async Task CargarAsync()
    {
        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var items = await _repo.ListarAsync();
            Categorias.Clear();
            foreach (var c in items) Categorias.Add(c);
        }
        catch (Exception ex) { ErrorMessage = $"Error al cargar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(NombreCategoria))
        {
            ErrorMessage = "El nombre es obligatorio.";
            return;
        }
        ErrorMessage = null;
        try
        {
            if (_isEditing)
                await _repo.ActualizarAsync(_editingId, NombreCategoria.Trim(), Descripcion?.Trim());
            else
                await _repo.CrearAsync(NombreCategoria.Trim(), Descripcion?.Trim());
            LimpiarFormulario();
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al guardar: {ex.Message}"; }
    }

    [RelayCommand]
    private void EditarSeleccionada()
    {
        if (CategoriaSeleccionada is null) return;
        _isEditing = true;
        _editingId = CategoriaSeleccionada.CategoriaID;
        NombreCategoria = CategoriaSeleccionada.NombreCategoria;
        Descripcion = CategoriaSeleccionada.Descripcion;
    }

    [RelayCommand]
    private async Task EliminarSeleccionadaAsync()
    {
        if (CategoriaSeleccionada is null) return;
        var res = MessageBox.Show($"¿Eliminar la categoría \"{CategoriaSeleccionada.NombreCategoria}\"?",
            "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (res != MessageBoxResult.Yes) return;
        ErrorMessage = null;
        try
        {
            await _repo.EliminarAsync(CategoriaSeleccionada.CategoriaID);
            await CargarAsync();
        }
        catch (Exception ex) { ErrorMessage = $"Error al eliminar: {ex.Message}"; }
    }

    [RelayCommand]
    private void CancelarEdicion() => LimpiarFormulario();

    private void LimpiarFormulario()
    {
        _isEditing = false;
        _editingId = 0;
        NombreCategoria = string.Empty;
        Descripcion = null;
        CategoriaSeleccionada = null;
    }
}
