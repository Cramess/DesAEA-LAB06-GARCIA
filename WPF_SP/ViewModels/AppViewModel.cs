using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WPF_SP.ViewModels;

public enum SeccionActiva { Categorias, Proveedores, Productos, Pedidos, Reportes }

public partial class AppViewModel : ObservableObject
{
    public CategoriasViewModel  Categorias  { get; }
    public ProveedoresViewModel Proveedores { get; }
    public ProductosViewModel   Productos   { get; }
    public PedidosViewModel     Pedidos     { get; }
    public ReportesViewModel    Reportes    { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowCategorias), nameof(ShowProveedores),
                              nameof(ShowProductos), nameof(ShowPedidos), nameof(ShowReportes))]
    private SeccionActiva seccionActual = SeccionActiva.Categorias;

    public bool ShowCategorias  => SeccionActual == SeccionActiva.Categorias;
    public bool ShowProveedores => SeccionActual == SeccionActiva.Proveedores;
    public bool ShowProductos   => SeccionActual == SeccionActiva.Productos;
    public bool ShowPedidos     => SeccionActual == SeccionActiva.Pedidos;
    public bool ShowReportes    => SeccionActual == SeccionActiva.Reportes;

    public AppViewModel(
        CategoriasViewModel  categorias,
        ProveedoresViewModel proveedores,
        ProductosViewModel   productos,
        PedidosViewModel     pedidos,
        ReportesViewModel    reportes)
    {
        Categorias  = categorias;
        Proveedores = proveedores;
        Productos   = productos;
        Pedidos     = pedidos;
        Reportes    = reportes;
    }

    [RelayCommand]
    private async Task NavegrarAsync(string seccion)
    {
        var nueva = seccion switch
        {
            "Categorias"  => SeccionActiva.Categorias,
            "Proveedores" => SeccionActiva.Proveedores,
            "Productos"   => SeccionActiva.Productos,
            "Pedidos"     => SeccionActiva.Pedidos,
            "Reportes"    => SeccionActiva.Reportes,
            _             => SeccionActual
        };

        SeccionActual = nueva;
        switch (nueva)
        {
            case SeccionActiva.Categorias:  await Categorias.CargarAsync();  break;
            case SeccionActiva.Proveedores: await Proveedores.CargarAsync(); break;
            case SeccionActiva.Productos:   await Productos.CargarAsync();   break;
            case SeccionActiva.Pedidos:     await Pedidos.CargarAsync();     break;
        }
    }
}
