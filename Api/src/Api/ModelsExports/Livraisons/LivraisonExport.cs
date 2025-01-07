using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed record LivraisonExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("numero")]
    public required string Numero { get; init; }

    [JsonPropertyName("fraisHt")]
    public required decimal FraisHT { get; init; }

    [JsonPropertyName("date")]
    public required DateOnly Date { get; init; }
}

[JsonSerializable(typeof(LivraisonExport[]))]
public partial class LivraisonExportContext: JsonSerializerContext { }
