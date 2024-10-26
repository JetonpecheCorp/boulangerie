using System.Text.Json.Serialization;

namespace Api.ModelsImports.Groupes;

public sealed record GroupeImport
{
    public required string Nom { get; init; }
    public required string Adresse { get; init; }
}

[JsonSerializable(typeof(GroupeImport))]
public partial class GroupeImportContext : JsonSerializerContext { }