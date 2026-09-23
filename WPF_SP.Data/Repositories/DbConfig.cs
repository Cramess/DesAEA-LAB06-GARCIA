using System.Configuration;

namespace WPF_SP.Data.Repositories;

public static class DbConfig
{
    public static string ConnectionString =>
        ConfigurationManager.ConnectionStrings["NeptunoDB"]?.ConnectionString
        ?? throw new InvalidOperationException(
            "No se encontró la cadena de conexión 'NeptunoDB' en App.config. " +
            "Asegúrese de que el archivo App.config del proyecto de inicio (WPF) " +
            "contenga la sección <connectionStrings> con la entrada 'NeptunoDB'.");
}
