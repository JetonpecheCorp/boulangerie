using System.Text.Json.Serialization;

namespace Api.ModelsExports.Tvas;

public sealed record TvaExport
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("valeur")]
    public required decimal Valeur { get; init; }
}

[JsonSerializable(typeof(TvaExport))]
[JsonSerializable(typeof(TvaExport[]))]
public partial class TvaExportContext: JsonSerializerContext { }
