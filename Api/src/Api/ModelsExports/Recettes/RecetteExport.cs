using System.Text.Json.Serialization;

namespace Api.ModelsExports.Recettes;

public sealed record RecetteExport
{
    [JsonPropertyName("idPublicIngredient")]
    public required string IdPublicIngredient { get; init; }

    [JsonPropertyName("idPublicProduit")]
    public required string IdPublicProduit { get; init; }

    [JsonPropertyName("nomProduit")]
    public required string NomProduit { get; init; }

    [JsonPropertyName("nomIngredient")]
    public required string NomIngredient { get; init; }

    [JsonPropertyName("quantite")]
    public required decimal Quantite { get; init; }
}

[JsonSerializable(typeof(RecetteExport[]))]
public partial class RecetteExportContext: JsonSerializerContext { }