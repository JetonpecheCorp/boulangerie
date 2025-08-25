using Api.ModelsImports.Vehicules;
using FluentValidation;

namespace Api.Validators.Vehicules;

public sealed class VehiculeImportValidator: AbstractValidator<VehiculeImport>
{
    public VehiculeImportValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Immatriculation).NotNull().MaximumLength(15);
        RuleFor(x => x.InfoComplementaire).MaximumLength(1_000);
    }
}
