using System.Text.Json.Serialization;

namespace Api.ModelsExports.Categories;

public sealed record CategorieExport
{
    [JsonPropertyName("idPublic")]
    public required string IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }
}

[JsonSerializable(typeof(CategorieExport))]
[JsonSerializable(typeof(CategorieExport[]))]
public partial class CategorieExportContext : JsonSerializerContext { }
