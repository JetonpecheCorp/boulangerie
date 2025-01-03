using System.Text.Json.Serialization;

namespace Api.ModelsImports;

public class PaginationImport
{
    public int NumPage { get; set; }
    public int NbParPage { get; set; }
    public string? ThermeRecherche { get; set; } = null;
}

[JsonSerializable(typeof(PaginationImport))]
public partial class PaginationImportContext: JsonSerializerContext { }
