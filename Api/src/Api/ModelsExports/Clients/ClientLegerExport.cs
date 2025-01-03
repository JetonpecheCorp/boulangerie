using System.Text.Json.Serialization;

namespace Api.ModelsExports.Clients;

public sealed record ClientLegerExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("adresse")]
    public required string Adresse { get; init; }
}

[JsonSerializable(typeof(ClientLegerExport[]))]
public partial class ClientLegerExportContext: JsonSerializerContext { }
