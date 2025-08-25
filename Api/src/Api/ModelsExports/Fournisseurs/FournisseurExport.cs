using System.Text.Json.Serialization;

namespace Api.ModelsExports.Fournisseurs;

public sealed record FournisseurExport
{
    public required Guid IdPublic { get; init; }
    public required string Nom { get; init; }
    public required IngredientFournisseur[] ListeIngredient { get; init; }
    public required IngredientFournisseur[] ListeProduit { get; init; }

    public string? Mail { get; init; }
    public string? Adresse { get; init; }
    public string? Telephone { get; init; }
}

public sealed record IngredientFournisseur
{
    public required Guid IdPublic { get; init; }
    public required string Nom { get; init; }
}

[JsonSerializable(typeof(FournisseurExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class FournisseurExportContext: JsonSerializerContext { }
