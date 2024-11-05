using Api.Extensions;
using Api.ModelsImports.Recettes;
using Api.Services.Ingredients;
using Api.Services.Produits;
using Api.Services.Recettes;
using FluentValidation;

namespace Api.Validators.Recettes;

public class RecetteImportValidator: AbstractValidator<RecetteImport>
{
    public RecetteImportValidator(
        IIngredientService _ingredientServ, 
        IProduitService _produitServ, 
        IRecetteService _recetteServ,
        IHttpContextAccessor _httpContext
    )
    {
        int idGroupe = _httpContext.HttpContext!.RecupererIdGroupe();

        RuleFor(x => x.Quantite).GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .NotEmpty()
            .MustAsync(async (x, _) => await _produitServ.ExisteAsync(x.IdPublicProduit, idGroupe))
            .WithMessage("Le produit n'existe pas")

            .NotEmpty()
            .MustAsync(async (x, _) => await _ingredientServ.ExisteAsync(x.IdPublicIngredient, idGroupe))
            .WithMessage("L'ingredient n'existe pas")

            .NotEmpty()
            .MustAsync(async (x, _) => !await _recetteServ.ExisteAsync(x.IdPublicProduit, x.IdPublicIngredient, idGroupe))
            .WithMessage("L'ingredient existe deja dans la recette du produit");
    }
}
