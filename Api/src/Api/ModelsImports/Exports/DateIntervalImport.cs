using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Exports;

public sealed record DateIntervalImport
{
    public required DateOnly DateDebut { get; init; }
    public required DateOnly DateFin { get; init; }
    public required EStatusCommande Status { get; init; }
    public required bool EstFormatExcel { get; init; }
}

[JsonSerializable(typeof(DateIntervalImport))]
public partial class DateIntervalImportContext: JsonSerializerContext { }
