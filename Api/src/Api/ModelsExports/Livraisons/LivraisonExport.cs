using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed record LivraisonExport
{
    public required Guid IdPublic { get; init; }

    public required string Numero { get; init; }

    public required decimal FraisHT { get; init; }

    public required DateOnly Date { get; init; }
}

[JsonSerializable(typeof(LivraisonExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class LivraisonExportContext: JsonSerializerContext { }
