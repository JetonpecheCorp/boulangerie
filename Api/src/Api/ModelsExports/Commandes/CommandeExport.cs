using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Commandes;

public sealed record CommandeExport
{
    [JsonPropertyName("numero")]
    public required string Numero { get; init; }

    [JsonPropertyName("date")]
    public required DateTime Date { get; init; }

    [JsonPropertyName("estLivraison")]
    public required bool EstLivraison { get; init; }

    [JsonPropertyName("client")]
    public CommandeClientExport? Client { get; init; }

    [JsonPropertyName("livraison")]
    public CommandeLivraisonExport? Livraison { get; init; }

    [JsonPropertyName("listeProduit")]
    public required CommandeProduitExport[] ListeProduit { get; init; }

    [JsonPropertyName("status")]
    public required EStatusCommande Status { get; init; }
}

public sealed record CommandeLivraisonExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("ordre")]
    public required int Ordre { get; init; }
}

public sealed record CommandeClientExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("adresse")]
    public required string Adresse { get; init; }
}

public sealed record CommandeProduitExport
{
    [JsonPropertyName("idPublic")]
    public required Guid IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }

    [JsonPropertyName("quantite")]
    public required int Quantite { get; init; }
}

[JsonSerializable(typeof(CommandeExport[]))]
public partial class CommandeExportContext: JsonSerializerContext { }
