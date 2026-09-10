namespace WPF_SP.Models;

public class Transportista
{
    public int TransportistaID { get; set; }
    public string CompaniaNombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }

    public override string ToString() => CompaniaNombre;
}
