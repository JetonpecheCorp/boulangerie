using System.Text.Json.Serialization;

namespace Api.ModelsImports.Ingredients;

public sealed record IngredientImport
{
    public required string Nom { get; init; }
    public string? CodeInterne { get; init; }
    public decimal Stock { get; init; }
    public decimal StockAlert { get; init; }
}

[JsonSerializable(typeof(IngredientImport))]
public partial class IngredientImportContext: JsonSerializerContext { }