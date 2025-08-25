using System.Text.Json.Serialization;

namespace Api.ModelsExports;

public sealed record ErreurValidationCSV
{
    public required int NumeroLigne { get; init; }
    public required string NomHeader { get; init; }
    public required string Message { get; init; }
}

[JsonSerializable(typeof(List<ErreurValidationCSV>))]
public partial class ErreurValidationCSVContext: JsonSerializerContext { }