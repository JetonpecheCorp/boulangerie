using System.Text.Json.Serialization;

namespace Api.ModelsExports.Ingredients;

public sealed record IngredientExport
{
    [JsonPropertyName("idPublic")]
    public required string IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("codeInterne")]
    public string? CodeInterne { get; init; }

    [JsonPropertyName("stock")]
    public required decimal Stock { get; init; }

    [JsonPropertyName("stockAlert")]
    public required decimal StockAlert { get; init; }
}

[JsonSerializable(typeof(IngredientExport[]))]
public partial class IngredientExportContext: JsonSerializerContext { }
