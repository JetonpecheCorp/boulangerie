using Api.ModelsImports.Fournisseurs;
using FluentValidation;

namespace Api.Validators.Fournisseurs;

public class FournisseurImportValidator: AbstractValidator<FournisseurImport>
{
    public FournisseurImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Adresse).MaximumLength(800);
        RuleFor(x => x.Telephone).MaximumLength(20);
        RuleFor(x => x.Mail).MaximumLength(250)
            .When(x => x.Mail is not null)
            .EmailAddress();
    }
}
