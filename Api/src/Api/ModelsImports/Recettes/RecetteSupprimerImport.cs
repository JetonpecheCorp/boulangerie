namespace Api.ModelsImports.Recettes;

public sealed record RecetteSupprimerImport
{
    public required string IdPublicProduit { get; init; }
    public required string IdPublicIngredient { get; init; }
}
