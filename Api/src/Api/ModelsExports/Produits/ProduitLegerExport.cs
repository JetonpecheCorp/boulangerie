using System.Text.Json.Serialization;

namespace Api.ModelsExports.Produits;

public sealed record ProduitLegerExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("prixHt")]
    public required decimal PrixHt { get; init; }
}

[JsonSerializable(typeof(ProduitLegerExport))]
public partial class ProduitLegerExportContext: JsonSerializerContext { }
