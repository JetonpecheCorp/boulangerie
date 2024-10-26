using System.Text.Json.Serialization;

namespace Api.ModelsExports.Groupes;

public sealed record GroupeExport
{
    public required int Id { get; init; }
    public required string Nom { get; init; }
}

[JsonSerializable(typeof(GroupeExport[]))]
public partial class GroupeExportContext: JsonSerializerContext { }
