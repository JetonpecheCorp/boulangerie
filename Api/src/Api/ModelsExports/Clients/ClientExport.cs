using System.Text.Json.Serialization;

namespace Api.ModelsExports.Clients;

public sealed record ClientExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("mail")]
    public string? Mail { get; init; }

    [JsonPropertyName("telephone")]
    public string? Telephone { get; init; }

    [JsonPropertyName("adresse")]
    public required string Adresse { get; init; }

    [JsonPropertyName("adresseFacturation")]
    public required string AdresseFacturation { get; init; }

    [JsonPropertyName("infoComplementaire")]
    public required string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(ClientExport[]))]
public partial class ClientExportContext : JsonSerializerContext { }
