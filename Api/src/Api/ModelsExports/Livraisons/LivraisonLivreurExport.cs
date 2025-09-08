using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed class LivraisonLivreurExport
{
    public required Guid IdPublic { get; init; }
    public required LivraisonVehiculeExport Vehicule { get; init; }
    public required LivreurCommandeExport[] ListeCommande { get; init; }
}

public sealed class LivreurCommandeExport
{
    public required string Numero { get; init; }
    public LivreurCommandeClientExport? Client { get; init; }
    public required LivreurCommandeProduitExport[] ListeProduit { get; init; }
    public required EStatusCommande Status { get; init; }
}

public sealed class LivreurCommandeClientExport
{
    public required string Nom { get; init; }
    public required string Adresse { get; init; }
}

public sealed record LivreurCommandeProduitExport
{
    public required string Nom { get; init; }
    public required int Quantite { get; init; }
}

[JsonSerializable(typeof(LivraisonLivreurExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class LivraisonLivreurExportContext: JsonSerializerContext { }