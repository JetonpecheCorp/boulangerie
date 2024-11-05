namespace Api.ModelsImports.Recettes;

public sealed record RecetteImport
{
    public required string IdPublicProduit { get; init; }
    public required string IdPublicIngredient { get; init; }
    public required decimal Quantite { get; init; }
}
