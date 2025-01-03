using System.Text.Json.Serialization;

namespace Api.ModelsExports.Livraisons;

public sealed record LivraisonExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("numero")]
    public required string Numero { get; init; }

    [JsonPropertyName("fraisHt")]
    public required decimal FraisHT { get; init; }

    [JsonPropertyName("vehicule")]
    public required LivraisonVehiculeExport Vehicule { get; init; }

    [JsonPropertyName("conducteur")]
    public required LivraisonConducteurExport Conducteur { get; init; }

    [JsonPropertyName("dateLivraison")]
    public required DateOnly Date { get; init; }
}

public sealed record LivraisonConducteurExport
{
    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("prenom")]
    public required string Prenom { get; init; }
}

public sealed record LivraisonVehiculeExport
{
    [JsonPropertyName("idPublic")]
    public Guid IdPublic { get; init; }

    [JsonPropertyName("immatriculation")]
    public required string Immatriculation { get; init; }

    [JsonPropertyName("infoComplementaire")]
    public string? InfoComplementaire { get; init; }
}

[JsonSerializable(typeof(LivraisonExport[]))]
public partial class LivraisonExportContext: JsonSerializerContext { }
