using System.Text.Json.Serialization;

namespace Api.ModelsImports.Commandes;

public sealed record CommandeImport
{
    public required string IdPublicClient { get; init; }
    public required ProduitCommandeImport[] ListeProduitCommande { get; init; }
}

public sealed record ProduitCommandeImport
{
    public required string IdPublicProduit { get; init; }
    public required int Quantite { get; init; }
}

[JsonSerializable(typeof(CommandeImport))]
public partial class CommandeImportContext: JsonSerializerContext { }
