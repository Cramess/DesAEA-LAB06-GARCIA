namespace WPF_SP.Data.Models;

public class Categoria
{
    public int CategoriaID { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public override string ToString() => NombreCategoria;
}
