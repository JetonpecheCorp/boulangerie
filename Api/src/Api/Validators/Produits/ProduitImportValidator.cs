using Api.Extensions;
using Api.ModelsImports.Produits;
using Api.Services.Categories;
using Api.Services.Tvas;
using FluentValidation;

namespace Api.Validators.Produits;

public class ProduitImportValidator: AbstractValidator<ProduitImport>
{
    public ProduitImportValidator(ITvaService _tvaServ, ICategorieService _categorieServ, IHttpContextAccessor _context)
    {
        RuleFor(x => x.CodeInterne).MaximumLength(100);
        RuleFor(x => x.PrixHt).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Nom).MaximumLength(300);
        RuleFor(x => x.StockAlert).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);

        RuleFor(x => x).Custom((x, context) =>
        {
            if (x.Mode == Enums.EModeImport.Modifier)
            {
                if (!x.IdPublic.HasValue || x.IdPublic.Value == Guid.Empty)
                    context.AddFailure("IdPublic", "IdPublic ne peux pas être null ou vide");
            }
        });

        RuleFor(x => x.Poids).Custom((poids, context) =>
        {
            if (poids is not null && poids < 0)
                context.AddFailure(nameof(poids), $"'{nameof(poids)}' ne peux pas être négatif");
        });

        RuleFor(x => x.IdTva)
            .NotEmpty()
            .MustAsync(async (idTva, _) => await _tvaServ.ExisteAsync(idTva))
            .WithMessage("La 'tva' n'existe pas");

        RuleFor(x => x.IdPublicCategorie)
            .NotEmpty()
            .MustAsync(async (idCategorie, _) => await _categorieServ.ExisteAsync(idCategorie, _context.HttpContext!.RecupererIdGroupe()))
            .WithMessage("La 'categorie' n'existe pas");
    }
}
