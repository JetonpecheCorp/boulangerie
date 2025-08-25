namespace Api.ModelsImports.Recettes;

public sealed record RecetteSupprimerImport
{
    public required Guid IdPublicProduit { get; init; }
    public required Guid IdPublicIngredient { get; init; }
}
