using Api.Extensions;
using Api.ModelsImports.Livraisons;
using Api.Services.Commandes;
using Api.Services.Utilisateurs;
using Api.Services.Vehicules;
using FluentValidation;

namespace Api.Validators.Livraisons;

public class LivraisonImportValidator: AbstractValidator<LivraisonImport>
{
    public LivraisonImportValidator(
        IUtilisateurService _utilisateurServ, 
        ICommandeService _commandeServ,
        IVehiculeService _vehiculeServ,
        IHttpContextAccessor _httpContextAccessor
    )
    {
        int idGroupe = _httpContextAccessor.HttpContext!.RecupererIdGroupe();

        RuleFor(x => x.Frais).GreaterThanOrEqualTo(0);

        RuleFor(x => x.IdPublicConducteur)
            .MustAsync(async (idPublic, _) => await _utilisateurServ.ExisteAsync(idPublic, idGroupe))
            .WithMessage("L'utilisateur n'existe pas");

        RuleFor(x => x.IdPublicVehicule)
            .MustAsync(async (idPublic, _) => await _vehiculeServ.ExisteAsync(idPublic, idGroupe))
            .WithMessage("Le véhicule n'existe pas");

        RuleForEach(x => x.Liste).ChildRules(x =>
        {
            x.RuleFor(y => y.Ordre).GreaterThanOrEqualTo(0);

            x.RuleFor(y => y.Numero)
                .NotEmpty()
                .MustAsync(async (numero, _) => await _commandeServ.ExisteAsync(numero, idGroupe))
                .WithMessage("Le numero de commande n'existe pas");
        });
    }
}
