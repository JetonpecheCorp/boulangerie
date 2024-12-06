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
                .NotEmpty()
                .MustAsync(async (idPublic, _) => await _clientServ.ExisteAsync(idPublic, idGroupe))
                .WithMessage("Le client n'existe pas");

            RuleForEach(x => x.ListeProduitCommande).NotEmpty().ChildRules(x =>
            {
                x.RuleFor(x => x.Quantite).GreaterThan(0);

                x.RuleFor(y => y.IdPublicProduit)
                    .NotEmpty()
                    .MustAsync(async (idPublic, _) => await _produitServ.ExisteAsync(idPublic, idGroupe))
                    .WithMessage("Le produit n'existe pas");
            });
        }
    }
}
