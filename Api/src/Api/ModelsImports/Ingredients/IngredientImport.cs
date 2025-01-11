using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Ingredients;

public sealed record IngredientImport
{
    public Guid? IdPublic { get; init; } = null;
    public required string Nom { get; init; }
    public string? CodeInterne { get; init; }
    public decimal Stock { get; init; }
    public decimal StockAlert { get; init; }

    [JsonIgnore]
    public EModeImport Mode { get; set; } = EModeImport.Ajouter;
}

[JsonSerializable(typeof(IngredientImport))]
public partial class IngredientImportContext: JsonSerializerContext { }