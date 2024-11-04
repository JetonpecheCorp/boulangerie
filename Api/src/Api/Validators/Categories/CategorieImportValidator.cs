using Api.ModelsImports.Categories;
using FluentValidation;

namespace Api.Validators.Categories;

public class CategorieImportValidator: AbstractValidator<CategorieImport>
{
    public CategorieImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);

        RuleFor(x => x).Custom((x, context) =>
        {
            if (x.Mode == Enums.EModeImport.Modifier && string.IsNullOrEmpty(x.IdPublic))
                context.AddFailure("idPublic", "'idPublic' ne peut pas être vide ou null");
        });
    }
}
