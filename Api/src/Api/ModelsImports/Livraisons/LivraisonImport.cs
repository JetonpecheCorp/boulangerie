using System.Text.Json.Serialization;

namespace Api.ModelsImports.Livraisons;

public sealed record LivraisonImport
{
    public required DateOnly Date { get; init; }
    public required Guid IdPublicConducteur { get; init; }
    public required Guid IdPublicVehicule { get; init; }
    public decimal Frais { get; init; }
    public required LivraisonCommande[] Liste { get; init; }
}

public sealed record LivraisonCommande
{
    public required string Numero { get; init; }
    public required int Ordre { get; init; }
}

[JsonSerializable(typeof(LivraisonImport))]
public partial class LivraisonImportContext: JsonSerializerContext { }
