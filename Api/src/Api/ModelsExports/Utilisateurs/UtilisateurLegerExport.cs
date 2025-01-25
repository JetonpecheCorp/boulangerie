using System.Text.Json.Serialization;

namespace Api.ModelsExports.Utilisateurs;

public record UtilisateurLegerExport
{
    public required Guid IdPublic { get; init; }

    public required string NomComplet { get; init; }
}

[JsonSerializable(typeof(UtilisateurLegerExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class UtilisateurLegerExportContext: JsonSerializerContext { }
