using Api.Extensions;
using Api.ModelsImports.Commandes;
using Api.Services.Clients;
using Api.Services.Produits;
using FluentValidation;

namespace Api.Validators.Commandes
{
    public sealed class CommandeImportValidator: AbstractValidator<CommandeImport>
    {
        public CommandeImportValidator(
            IClientService _clientServ,
            IProduitService _produitServ,
            IHttpContextAccessor _httpContextAccessor)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            int idGroupe = _httpContextAccessor.HttpContext!.RecupererIdGroupe();

            RuleFor(x => x.IdPublicClient)
                .MustAsync(async (idPublic, _) => idPublic.HasValue ? await _clientServ.ExisteAsync(idPublic.Value, idGroupe) : true)
                .WithMessage("Le client n'existe pas");

            RuleForEach(x => x.ListeProduit).NotEmpty().ChildRules(x =>
            {
                x.RuleFor(x => x.Quantite).GreaterThan(0);

                x.RuleFor(y => y.IdPublic)
                    .NotEmpty()
                    .MustAsync(async (idPublic, _) => await _produitServ.ExisteAsync(idPublic, idGroupe))
                    .WithMessage("Le produit n'existe pas");
            });
        }
    }
}
