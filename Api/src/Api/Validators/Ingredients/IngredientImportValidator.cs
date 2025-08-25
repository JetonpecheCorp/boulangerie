using Api.ModelsImports.Ingredients;
using FluentValidation;

namespace Api.Validators.Ingredients;

public class IngredientImportValidator: AbstractValidator<IngredientImport>
{
    public IngredientImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CodeInterne).MaximumLength(100);
        RuleFor(x => x.StockAlert).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);

        RuleFor(x => x).Custom((x, context) =>
        {
            if(x.Mode == Enums.EModeImport.Modifier)
            {
                if (x.IdPublic == Guid.Empty)
                    context.AddFailure("IdPublic", "IdPublic ne peux pas être null ou vide");
            }
        });
    }
}
