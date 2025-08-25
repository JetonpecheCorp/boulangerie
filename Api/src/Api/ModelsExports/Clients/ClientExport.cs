using System.Text.Json.Serialization;

namespace Api.ModelsExports.Clients;

public sealed record ClientExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public string? Mail { get; init; }

    public string? Telephone { get; init; }

    public required string Adresse { get; init; }

    public required string AdresseFacturation { get; init; }

    public required string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(ClientExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class ClientExportContext : JsonSerializerContext { }
