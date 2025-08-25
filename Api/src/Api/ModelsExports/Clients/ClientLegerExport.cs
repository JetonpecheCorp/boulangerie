using System.Text.Json.Serialization;

namespace Api.ModelsExports.Clients;

public sealed record ClientLegerExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public required string Adresse { get; init; }
}

[JsonSerializable(typeof(ClientLegerExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class ClientLegerExportContext: JsonSerializerContext { }
