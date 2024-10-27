using Api.ModelsExports.Ingredients;
using System.Text.Json.Serialization;

namespace Api.ModelsExports;

public sealed record PaginationExport<T> where T : class
{
    [JsonPropertyName("liste")]
    public required T[] Liste { get; init; }

    [JsonPropertyName("numPage")]
    public int NumPage { get; init; }

    [JsonPropertyName("nbParPage")]
    public int NbParPage { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("aUneProchainePage")]
    public bool AUneProchainePage => Total > (NumPage * NbParPage);
}

[JsonSerializable(typeof(PaginationExport<IngredientExport>))]
public partial class PaginationExportContext: JsonSerializerContext { }
