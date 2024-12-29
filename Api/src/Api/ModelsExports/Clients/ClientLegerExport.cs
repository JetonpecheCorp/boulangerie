using System.Text.Json.Serialization;

namespace Api.ModelsExports.Clients;

public sealed record ClientLegerExport
{
    [JsonPropertyName("idPublic")]
    public required string IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }
}

[JsonSerializable(typeof(ClientLegerExport[]))]
public partial class ClientLegerExportContext: JsonSerializerContext { }
