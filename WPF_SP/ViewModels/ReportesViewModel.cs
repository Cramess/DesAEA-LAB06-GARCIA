using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPF_SP.Data.Models;
using WPF_SP.Data.Repositories;

namespace WPF_SP.ViewModels;

public partial class ReportesViewModel : ObservableObject
{
    private readonly IPedidoRepository _repo;

    public ObservableCollection<DetallePedidoReporte> Detalles { get; } = new();

    [ObservableProperty] private DateTime fechaInicio = new(2026, 8, 1);
    [ObservableProperty] private DateTime fechaFin    = new(2026, 8, 31);
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private bool hasData;
    [ObservableProperty] private decimal totalVentas;
    [ObservableProperty] private int totalRegistros;
    [ObservableProperty] private decimal totalUnidades;

    public ReportesViewModel(IPedidoRepository repo) => _repo = repo;

    [RelayCommand]
    public async Task GenerarReporteAsync()
    {
        if (FechaInicio > FechaFin)
        {
            ErrorMessage = "La fecha de inicio no puede ser mayor que la fecha fin.";
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var items = await _repo.ObtenerReporteAsync(FechaInicio, FechaFin);
            Detalles.Clear();
            foreach (var d in items) Detalles.Add(d);

            TotalRegistros = Detalles.Count;
            TotalVentas    = Detalles.Sum(d => d.SubTotal);
            TotalUnidades  = Detalles.Sum(d => d.Cantidad);
            HasData        = Detalles.Count > 0;
        }
        catch (Exception ex) { ErrorMessage = $"Error al generar reporte: {ex.Message}"; }
        finally { IsBusy = false; }
    }
}
