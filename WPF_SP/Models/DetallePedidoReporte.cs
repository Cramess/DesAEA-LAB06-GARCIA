namespace WPF_SP.Models;

public class DetallePedidoReporte
{
    public int PedidoID { get; set; }
    public DateTime FechaPedido { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public string? Destinatario { get; set; }
    public string? CiudadDestino { get; set; }
    public string? PaisDestino { get; set; }
    public string? NombreCliente { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public decimal PrecioUnidad { get; set; }
    public short Cantidad { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}
