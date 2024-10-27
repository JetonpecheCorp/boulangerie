using Api.ModelsExports.Ingredients;
using System.Text.Json.Serialization;

namespace Api.ModelsExports;

public sealed record PaginationExport<T> where T : class
{
    public required T[] Liste { get; init; }

    public int NumPage { get; init; }
    public int NbParPage { get; init; }
    public int Total { get; init; }
    public bool AUneProchainePage => Total > (NumPage * NbParPage);
}

[JsonSerializable(typeof(PaginationExport<IngredientExport>))]
public partial class PaginationExportContext: JsonSerializerContext { }
