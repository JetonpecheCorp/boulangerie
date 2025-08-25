using System.Text.Json.Serialization;

namespace Api.ModelsExports.Vehicules;

public sealed record VehiculeExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public required string Immatriculation { get; init; }

    public required string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(VehiculeExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class VehiculeExportContext: JsonSerializerContext { }
