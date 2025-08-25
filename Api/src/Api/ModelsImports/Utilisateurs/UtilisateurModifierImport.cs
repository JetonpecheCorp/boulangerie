using System.Text.Json.Serialization;

namespace Api.ModelsImports.Utilisateurs;

public sealed record UtilisateurModifierImport
{
    public required string Nom { get; init; }
    public required string Prenom { get; init; }
    public required string Mail { get; init; }
    public string? Telephone { get; init; }

    [JsonIgnore]
    public Guid IdPublic { get; set; }
}

[JsonSerializable(typeof(UtilisateurModifierImport))]
public partial class UtilisateurModifierImportContext : JsonSerializerContext { }
