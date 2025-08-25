using Api.ModelsExports.Categories;
using Api.ModelsExports.Tvas;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Produits;

public sealed record ProduitExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public string[] ListeAllergene { get; set; } = [];

    [JsonIgnore]
    public string? Allergene { get; init; }

    public string? CodeInterne { get; init; }

    public required decimal Stock { get; init; }

    public decimal? Poids { get; init; }

    public required decimal StockAlert { get; init; }

    public required decimal PrixHt { get; init; }

    public required TvaExport Tva { get; init; }

    public required CategorieExport Categorie { get; init; }
}

[JsonSerializable(typeof(ProduitExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class ProduitExportContext: JsonSerializerContext { }
