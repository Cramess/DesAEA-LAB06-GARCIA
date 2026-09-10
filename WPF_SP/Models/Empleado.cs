namespace WPF_SP.Models;

public class Empleado
{
    public int EmpleadoID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellidos}";

    public override string ToString() => NombreCompleto;
}
