using Api.ModelsImports.Ingredients;
using FluentValidation;

namespace Api.Validators.Ingredients;

public class IngredientImportValidator: AbstractValidator<IngredientImport>
{
    public IngredientImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CodeInterne).MaximumLength(100);
    }
}
