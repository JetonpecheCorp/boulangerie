using Api.Extensions;
using Api.ModelsImports.Recettes;
using Api.Services.Ingredients;
using Api.Services.Produits;
using Api.Services.Recettes;
using FluentValidation;

namespace Api.Validators.Recettes;

public class RecetteModifierQteImportValidator: AbstractValidator<RecetteModifierQteImport>
{
    public RecetteModifierQteImportValidator(
        IProduitService _produitServ, 
        IIngredientService _ingredientServ,
        IHttpContextAccessor _httpContextAccessor
    )
    {
        int idGroupe = _httpContextAccessor.HttpContext!.RecupererIdGroupe();

        RuleFor(x => x.IdPublicProduit)
            .NotEmpty()
            .MustAsync(async (idPublicProduit, _) => await _produitServ.ExisteAsync(idPublicProduit, idGroupe))
            .WithMessage("Le produit n'existe pas");

        RuleFor(x => x.IdPublicIngredient)
            .NotEmpty()
            .MustAsync(async (idPublicIngredient, _) => await _ingredientServ.ExisteAsync(idPublicIngredient, idGroupe))
            .WithMessage("L'ingredient n'existe pas");

        RuleFor(x => x.Quantite).GreaterThanOrEqualTo(0);
    }
}
