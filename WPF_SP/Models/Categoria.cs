namespace WPF_SP.Models;

public class Categoria
{
    public int CategoriaID { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public override string ToString() => NombreCategoria;
}
