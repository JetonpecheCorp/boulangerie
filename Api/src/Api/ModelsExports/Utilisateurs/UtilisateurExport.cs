using System.Text.Json.Serialization;

namespace Api.ModelsExports.Utilisateurs;

public sealed record UtilisateurExport
{
    public required string Nom { get; init; }

    public required string Prenom { get; init; }

    public required string Mail { get; init; }

    public required bool EstAdmin { get; init; }

    public string? Telephone { get; init; }
}

[JsonSerializable(typeof(UtilisateurExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UtilisateurExportContext: JsonSerializerContext { }
