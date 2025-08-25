using System.Text.Json.Serialization;

namespace Api.ModelsExports.Produits;

public sealed record ProduitLegerExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public required decimal PrixHt { get; init; }
}

[JsonSerializable(typeof(ProduitLegerExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class ProduitLegerExportContext: JsonSerializerContext { }
