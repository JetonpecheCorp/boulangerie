using System.Text.Json.Serialization;

namespace Api.ModelsExports.Fournisseurs;

public sealed record FournisseurExport
{
    [JsonPropertyName("idPublic")]
    public required string IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("mail")]
    public string? Mail { get; init; }

    [JsonPropertyName("adresse")]
    public string? Adresse { get; init; }

    [JsonPropertyName("telephone")]
    public string? Telephone { get; init; }
}

[JsonSerializable(typeof(FournisseurExport))]
public partial class FournisseurExportContext: JsonSerializerContext { }
