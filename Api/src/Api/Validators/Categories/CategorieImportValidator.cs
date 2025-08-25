using Api.ModelsImports.Categories;
using FluentValidation;

namespace Api.Validators.Categories;

public class CategorieImportValidator: AbstractValidator<CategorieImport>
{
    public CategorieImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);
    }
}
