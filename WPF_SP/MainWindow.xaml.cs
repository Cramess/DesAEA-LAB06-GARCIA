using System.Windows;
using WPF_SP.Data;
using WPF_SP.ViewModels;

namespace WPF_SP
{
    public partial class MainWindow : Window
    {
        private readonly AppViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var cs = DbConfig.ConnectionString;

            var categoriaRepo  = new CategoriaRepository(cs);
            var proveedorRepo  = new ProveedorRepository(cs);
            var productoRepo   = new ProductoRepository(cs);
            var pedidoRepo     = new PedidoRepository(cs);

            _viewModel = new AppViewModel(
                new CategoriasViewModel(categoriaRepo),
                new ProveedoresViewModel(proveedorRepo),
                new ProductosViewModel(productoRepo, categoriaRepo, proveedorRepo),
                new PedidosViewModel(pedidoRepo),
                new ReportesViewModel(pedidoRepo)
            );

            DataContext = _viewModel;
            Loaded += async (_, _) =>
                await _viewModel.NavegrarCommand.ExecuteAsync("Categorias");
        }
    }
}
