using System.Text.Json.Serialization;

namespace Api.ModelsExports.Tvas;

public sealed record TvaExport
{
    public required int Id { get; init; }

    public required decimal Valeur { get; init; }
}

[JsonSerializable(typeof(TvaExport))]
[JsonSerializable(typeof(TvaExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class TvaExportContext: JsonSerializerContext { }
