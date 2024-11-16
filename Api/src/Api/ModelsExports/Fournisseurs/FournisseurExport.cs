using System.Text.Json.Serialization;

namespace Api.ModelsExports.Fournisseurs;

public sealed record FournisseurExport
{
    public required string IdPublic { get; init; }
    public required string Nom { get; init; }
    public string? Mail { get; init; }
    public string? Adresse { get; init; }
    public string? Telephone { get; init; }
}

[JsonSerializable(typeof(FournisseurExport))]
public partial class FournisseurExportContext: JsonSerializerContext { }
