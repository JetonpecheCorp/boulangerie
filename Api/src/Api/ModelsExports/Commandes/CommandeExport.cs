using Api.Enums;
using System.Text.Json.Serialization;

namespace Api.ModelsExports.Commandes;

public sealed record CommandeExport
{
    public required string Numero { get; init; }

    public required DateTime Date { get; init; }

    public required bool EstLivraison { get; init; }

    public CommandeClientExport? Client { get; init; }

    public CommandeLivraisonExport? Livraison { get; init; }

    public required CommandeProduitExport[] ListeProduit { get; init; }

    public required EStatusCommande Status { get; init; }
}

public sealed record CommandeLivraisonExport
{
    public required Guid IdPublic { get; init; }

    public required int Ordre { get; init; }
}

public sealed record CommandeClientExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public required string Adresse { get; init; }
}

public sealed record CommandeProduitExport
{
    public required Guid IdPublic { get; init; }

    public required string Nom { get; init; }

    public required int Quantite { get; init; }
}

[JsonSerializable(typeof(CommandeExport[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class CommandeExportContext: JsonSerializerContext { }
