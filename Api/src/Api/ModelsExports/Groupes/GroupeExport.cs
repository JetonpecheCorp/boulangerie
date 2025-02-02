using System.Text.Json.Serialization;

namespace Api.ModelsExports.Groupes;

public sealed record GroupeExport
{
    public required int Id { get; init; }
    public required string Nom { get; init; }
    public required string Adresse { get; init; }
    public required bool ConnexionBloquer { get; init; }
}

[JsonSerializable(typeof(GroupeExport[]))]
[JsonSerializable(typeof(GroupeExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class GroupeExportContext: JsonSerializerContext { }
