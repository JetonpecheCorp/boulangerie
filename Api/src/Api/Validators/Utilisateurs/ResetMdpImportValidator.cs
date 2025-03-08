using Api.Extensions;
using Api.ModelsImports.Utilisateurs;
using FluentValidation;

namespace Api.Validators.Utilisateurs;

public sealed class ResetMdpImportValidator: AbstractValidator<ResetMdpImport>
{
    public ResetMdpImportValidator()
    {
        RuleFor(x => x.Mdp).MotDePasse();
    }
}
