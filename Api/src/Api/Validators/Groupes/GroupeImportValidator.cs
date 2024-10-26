using Api.ModelsImports.Groupes;
using FluentValidation;

namespace Api.Validators.Groupes;

public class GroupeImportValidator: AbstractValidator<GroupeImport>
{
    public GroupeImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Adresse).NotEmpty().MaximumLength(800);
    }
}
