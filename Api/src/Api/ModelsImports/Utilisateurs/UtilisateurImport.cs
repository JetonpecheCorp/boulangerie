using System.Text.Json.Serialization;

namespace Api.ModelsImports.Utilisateurs;

public sealed record UtilisateurImport
{
    public required string Nom { get; init; }
    public required string Prenom { get; init; }
    public required string Mail { get; init; }
    public required string Mdp { get; init; }
    public string? Telephone { get; init; }
}

[JsonSerializable(typeof(UtilisateurImport))]
public partial class UtilisateurImportContext : JsonSerializerContext { }
