namespace Api.ModelsImports.Recettes;

public sealed record RecetteFiltreImport
{
    public string? IdPublicIngredient { get; init; }
    public string? IdPublicProduit { get; init; }
}
