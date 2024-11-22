using Api.Extensions;
using Api.ModelsImports.Fournisseurs;
using Api.Services.Ingredients;
using Api.Services.Produits;
using FluentValidation;

namespace Api.Validators.Fournisseurs;

public class FournisseurImportValidator: AbstractValidator<FournisseurImport>
{
    public FournisseurImportValidator(
        IIngredientService _ingredientServ, 
        IProduitService _produitServ, 
        IHttpContextAccessor _httpContextAccessor
    )
    {
        int idGroupe = _httpContextAccessor.HttpContext!.RecupererIdGroupe();

        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Adresse).MaximumLength(800);
        RuleFor(x => x.Telephone).MaximumLength(20);
        RuleFor(x => x.Mail).MaximumLength(250)
            .When(x => x.Mail is not null)
            .EmailAddress();

        RuleForEach(x => x.ListeIdPublicIngredient).ChildRules(x =>
        {
            x.RuleFor(y => y).MustAsync(async (idPublic, _) => await _ingredientServ.ExisteAsync(idPublic, idGroupe))
                .WithMessage("L'ingredient n'existe pas");
        });

        RuleForEach(x => x.ListeIdPublicProduit).ChildRules(x =>
        {
            x.RuleFor(y => y).MustAsync(async (idPublic, _) => await _produitServ.ExisteAsync(idPublic, idGroupe))
                .WithMessage("Le produit n'existe pas");
        });
    }
}
