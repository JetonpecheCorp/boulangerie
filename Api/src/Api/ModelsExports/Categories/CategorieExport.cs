using System.Text.Json.Serialization;

namespace Api.ModelsExports.Categories;

public sealed record CategorieExport
{
    public required string IdPublic { get; init; }

    public required string Nom { get; init; }
}

[JsonSerializable(typeof(CategorieExport))]
[JsonSerializable(typeof(CategorieExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class CategorieExportContext : JsonSerializerContext { }
