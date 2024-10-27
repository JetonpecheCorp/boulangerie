using System.Text.Json.Serialization;

namespace Api.ModelsImports.Utilisateurs;

public sealed record ConnexionImport
{
    public required string Login { get; init; }
    public required string Mdp { get; init; }
}

[JsonSerializable(typeof(ConnexionImport))]
public partial class ConnexionImportContext: JsonSerializerContext { }
