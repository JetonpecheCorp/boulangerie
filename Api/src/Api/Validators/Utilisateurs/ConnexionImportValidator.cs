using Api.ModelsImports.Utilisateurs;
using FluentValidation;

namespace Api.Validators.Utilisateurs;

public class ConnexionImportValidator: AbstractValidator<ConnexionImport>
{
    public ConnexionImportValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Mdp).NotEmpty();
    }
}
