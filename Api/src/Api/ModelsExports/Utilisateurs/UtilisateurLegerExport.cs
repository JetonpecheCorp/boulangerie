using System.Text.Json.Serialization;

namespace Api.ModelsExports.Utilisateurs;

public record UtilisateurLegerExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nomComplet")]
    public required string NomComplet { get; init; }
}

[JsonSerializable(typeof(UtilisateurLegerExport))]
public partial class UtilisateurLegerExportContext: JsonSerializerContext { }
