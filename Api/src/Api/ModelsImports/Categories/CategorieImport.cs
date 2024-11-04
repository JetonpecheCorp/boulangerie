using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Categories;

public sealed record CategorieImport
{
    public string? IdPublic { get; init; } = null;
    public required string Nom { get; init; }

    [JsonIgnore]
    public EModeImport Mode { get; set; } = EModeImport.Ajouter;
}

[JsonSerializable(typeof(CategorieImport))]
public partial class CategorieImportContext: JsonSerializerContext { }
