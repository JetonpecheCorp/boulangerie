using System.Text.Json.Serialization;

namespace Api.ModelsImports.Utilisateurs;

public sealed record ResetMdpImport
{
    public required string Mdp { get; init; }
}

[JsonSerializable(typeof(ResetMdpImport))]
public partial class ResetMdpImportContext: JsonSerializerContext { }
