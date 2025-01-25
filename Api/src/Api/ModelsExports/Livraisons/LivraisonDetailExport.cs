using Api.ModelsExports.Commandes;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed record LivraisonDetailExport
{
    public required LivraisonConducteurExport Conducteur { get; init; }

    public required LivraisonVehiculeExport Vehicule { get; init; }

    public required CommandeExport[] ListeCommande { get; init; }
}

public sealed record LivraisonConducteurExport
{
    public required Guid IdPublic { get; init; }

    public required string NomComplet { get; init; }
}

public sealed record LivraisonVehiculeExport
{
    public Guid IdPublic { get; init; }

    public required string Immatriculation { get; init; }

    public required string Nom { get; init; }
}

[JsonSerializable(typeof(LivraisonDetailExport))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class LivraisonDetailExportContext: JsonSerializerContext { }
