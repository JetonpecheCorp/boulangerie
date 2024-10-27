using System.Text.Json.Serialization;

namespace Api.ModelsExports.Connexions;

public sealed record ConnexionExport
{
    public required string Nom { get; init; }
    public required string Prenom { get; init; }
    public required string Jwt { get; init; }
}

[JsonSerializable(typeof(ConnexionExport))]
public partial class ConnexionExportContext: JsonSerializerContext { }
