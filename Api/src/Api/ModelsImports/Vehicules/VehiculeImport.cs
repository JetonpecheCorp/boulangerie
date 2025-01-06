
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Vehicules;

public sealed record VehiculeImport
{
    public required string Nom { get; init; }
    public string Immatriculation { get; init; } = "";
    public string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(VehiculeImport))]
public partial class VehiculeImportContext: JsonSerializerContext { }
