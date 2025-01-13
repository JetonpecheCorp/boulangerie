using System.Text.Json.Serialization;

namespace Api.ModelsImports.Livraisons;

public sealed class LivraisonFiltreImport: PaginationImport
{
    public DateOnly? DateDebut { get; init; }
    public DateOnly? DateFin { get; init; }
    public Guid? IdPublicClient { get; init; }
}

[JsonSerializable(typeof(LivraisonFiltreImport))]
public partial class LivraisonFiltreImportContext: JsonSerializerContext { }
