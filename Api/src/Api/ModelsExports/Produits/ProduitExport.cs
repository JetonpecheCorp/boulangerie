using Api.ModelsExports.Categories;
using Api.ModelsExports.Tvas;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Produits;

public sealed record ProduitExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("listeAllergene")]
    public string[] ListeAllergene { get; set; } = [];

    [JsonIgnore]
    public string? Allergene { get; init; }

    [JsonPropertyName("codeInterne")]
    public string? CodeInterne { get; init; }

    [JsonPropertyName("stock")]
    public required decimal Stock { get; init; }

    [JsonPropertyName("poids")]
    public decimal? Poids { get; init; }

    [JsonPropertyName("stockAlert")]
    public required decimal StockAlert { get; init; }

    [JsonPropertyName("prixHt")]
    public required decimal PrixHt { get; init; }

    [JsonPropertyName("tva")]
    public required TvaExport Tva { get; init; }

    [JsonPropertyName("categorie")]
    public required CategorieExport Categorie { get; init; }
}

[JsonSerializable(typeof(ProduitExport))]
public partial class ProduitExportContext: JsonSerializerContext { }
