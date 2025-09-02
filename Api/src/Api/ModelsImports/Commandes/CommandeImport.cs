using System.Text.Json.Serialization;

namespace Api.ModelsImports.Commandes;

public sealed record CommandeImport
{
    public Guid? IdPublicClient { get; init; }
    public bool EstLivraison { get; init; }
    public DateOnly Date { get; init; }
    public required ProduitCommandeImport[] ListeProduit { get; init; }
}

public sealed record ProduitCommandeImport
{
    public required Guid IdPublic { get; init; }
    public required int Quantite { get; init; }
}

[JsonSerializable(typeof(CommandeImport))]
public partial class CommandeImportContext: JsonSerializerContext { }
