using System.Text.Json.Serialization;

namespace Api.ModelsImports.Fournisseurs;

public sealed record FournisseurImport
{
    public required string Nom { get; init; }
    public string? Adresse { get; init; }
    public string? Telephone { get; init; }
    public string? Mail { get; init; }
}

[JsonSerializable(typeof(FournisseurImport))]
public partial class FournisseurImportContext: JsonSerializerContext { }
