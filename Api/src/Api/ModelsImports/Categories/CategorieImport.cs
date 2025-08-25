using System.Text.Json.Serialization;

namespace Api.ModelsImports.Categories;

public sealed record CategorieImport
{
    public required string Nom { get; init; }
} 

[JsonSerializable(typeof(CategorieImport))]
public partial class CategorieImportContext: JsonSerializerContext { }
