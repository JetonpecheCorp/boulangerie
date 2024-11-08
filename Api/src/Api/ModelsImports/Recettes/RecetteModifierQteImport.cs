using System.Text.Json.Serialization;

namespace Api.ModelsImports.Recettes;

public record RecetteModifierQteImport
{
    public required string IdPublicIngredient { get; init; }
    public decimal Quantite { get; init; }
}

[JsonSerializable(typeof(RecetteModifierQteImport))]
public partial class RecetteModifierQteImportContext: JsonSerializerContext { }
