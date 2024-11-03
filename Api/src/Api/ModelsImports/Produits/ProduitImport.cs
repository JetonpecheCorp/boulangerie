using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Produits;

public sealed record ProduitImport
{
    public string? IdPublic { get; init; }
    public required string IdPublicCategorie { get; init; }
    public required int IdTva { get; init; }
    public required string Nom { get; init; }
    public required decimal PrixHt { get; init; }
    public required decimal Stock { get; init; }
    public required decimal StockAlert { get; init; }
    public decimal? Poids { get; init; } = null;
    public string? CodeInterne { get; init; } = null;
    public string[] ListeAllergene { get; init; } = [];

    [JsonIgnore]
    public EModeImport Mode { get; set; } = EModeImport.Ajouter;
}

[JsonSerializable(typeof(ProduitImport))]
public partial class ProduitImportContext: JsonSerializerContext { }
