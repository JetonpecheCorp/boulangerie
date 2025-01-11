namespace Api.ModelsImports.Recettes;

public sealed record RecetteImport
{
    public required Guid IdPublicProduit { get; init; }
    public required Guid IdPublicIngredient { get; init; }
    public required decimal Quantite { get; init; }
}
