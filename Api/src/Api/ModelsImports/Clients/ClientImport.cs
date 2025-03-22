using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsImports;

public sealed record ClientImport
{
    [JsonIgnore]
    public Guid IdPublic { get; set; } = Guid.Empty;

    [JsonIgnore]
    public EModeImport Mode { get; set; } = EModeImport.Ajouter;

    public required string Nom { get; init; }
    public required string Adresse { get; init; }
    public string? AdresseFacturation { get; init; }
    public string? Mail { get; init; }
    public string? Telephone { get; init; }
    public string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(ClientImport))]
public partial class ClientImportContext: JsonSerializerContext { }
