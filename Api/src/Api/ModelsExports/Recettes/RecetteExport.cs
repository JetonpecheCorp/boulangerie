using System.Text.Json.Serialization;

namespace Api.ModelsExports.Recettes;

public sealed record RecetteExport
{
    public required Guid IdPublicIngredient { get; init; }

    public required Guid IdPublicProduit { get; init; }

    public required string NomProduit { get; init; }

    public required string NomIngredient { get; init; }

    public required decimal Quantite { get; init; }
}

[JsonSerializable(typeof(RecetteExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class RecetteExportContext: JsonSerializerContext { }