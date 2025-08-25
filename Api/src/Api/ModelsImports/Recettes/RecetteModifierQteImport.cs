using System.Text.Json.Serialization;

namespace Api.ModelsImports.Recettes;

public record RecetteModifierQteImport
{
    public required Guid IdPublicIngredient { get; init; }
    public required Guid IdPublicProduit { get; init; }
    public decimal Quantite { get; init; }
}

[JsonSerializable(typeof(RecetteModifierQteImport))]
public partial class RecetteModifierQteImportContext: JsonSerializerContext { }
