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
    public required CommandeClientExport? Client { get; init; }

    [JsonPropertyName("listeProduit")]
    public required CommandeProduitExport[] ListeProduit { get; init; }
}

public sealed record CommandeClientExport
{
    [JsonPropertyName("idPublic")]
    public required Guid? IdPublic { get; init; }

    [JsonPropertyName("nom")]
    public required string Nom { get; init; }
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
