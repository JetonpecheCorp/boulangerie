using System.Text.Json.Serialization;

namespace Api.ModelsExports.Vehicules;

public sealed record VehiculeExport
{
    [JsonPropertyName("idPublic")]
    public required string IdPublic { get; init; }

    [JsonPropertyName("immatriculation")]
    public required string Immatriculation { get; init; }

    [JsonPropertyName("infoComplementaire")]
    public required string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(VehiculeExport))]
public partial class VehiculeExportContext: JsonSerializerContext { }
