using System.Text.Json.Serialization;

namespace Api.ModelsImports.Livraisons;

public sealed class LivraisonFiltreImport: PaginationImport
{
    public DateOnly? Date { get; init; }
}

[JsonSerializable(typeof(LivraisonFiltreImport))]
public partial class LivraisonFiltreImportContext: JsonSerializerContext { }
