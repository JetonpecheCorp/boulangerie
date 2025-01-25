using System.Text.Json.Serialization;

namespace Api.ModelsExports.Ingredients;

public sealed record IngredientExport
{
    public required string IdPublic { get; init; }

    public required string Nom { get; init; }

    public string? CodeInterne { get; init; }

    public required decimal Stock { get; init; }

    public required decimal StockAlert { get; init; }
}

[JsonSerializable(typeof(IngredientExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class IngredientExportContext: JsonSerializerContext { }
