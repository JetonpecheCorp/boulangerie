using Api.ModelsExports.Commandes;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed record LivraisonDetailExport
{
    [JsonPropertyName("conducteur")]
    public required LivraisonConducteurExport Conducteur { get; init; }

    [JsonPropertyName("vehicule")]
    public required LivraisonVehiculeExport Vehicule { get; init; }

    public required CommandeExport[] ListeCommande { get; init; }
}

public sealed record LivraisonConducteurExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nomComplet")]
    public required string NomComplet { get; init; }
}

public sealed record LivraisonVehiculeExport
{
    [JsonPropertyName("idPublic")]
    public Guid IdPublic { get; init; }

    [JsonPropertyName("immatriculation")]
    public required string Immatriculation { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }
}

[JsonSerializable(typeof(LivraisonDetailExport))]
public partial class LivraisonDetailExportContext: JsonSerializerContext { }
