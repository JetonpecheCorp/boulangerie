using Api.Extensions;
using Api.Models;
using Api.ModelsImports.Commandes;
using Microsoft.EntityFrameworkCore;
using Api.Enums;
using Api.ModelsExports.Commandes;
using ProduitPrix = (System.Guid IdPublic, int Id, decimal PrixHt);
using Api.ModelsExports;

namespace Api.Services.Commandes;

public sealed class CommandeService(BoulangerieContext _context): ICommandeService
{
    public async Task<PaginationExport<CommandeExport>> ListerAsync(CommandeFiltreImport _filtre, int _idGroupe)
    {
        var requete = _context.Commandes
            .Where(x => x.IdGroupe == _idGroupe);

        if (!string.IsNullOrWhiteSpace(_filtre.ThermeRecherche))
            requete = requete.Where(x => x.Numero.Contains(_filtre.ThermeRecherche));

        else
        {
            if (_filtre.SansLivraison.HasValue)
            {
                // commande sans livraison
                if (_filtre.SansLivraison.Value)
                    requete = requete.Where(x => x.IdLivraison == null && x.EstLivraison);

                // commande avec livraison
                else
                    requete = requete.Where(x => x.IdLivraison != null);
            }

            requete = _filtre.Status switch
            {
                EStatusCommande.Valider => requete.Where(x => x.DateValidation.HasValue),
                EStatusCommande.EnAttenteValidation => requete.Where(x => !x.DateValidation.HasValue && !x.DateAnnulation.HasValue),
                EStatusCommande.Annuler => requete.Where(x => x.DateAnnulation.HasValue),
                EStatusCommande.Livrer => requete.Where(x => x.DatLivraison.HasValue),
                _ => requete
            };

            if (_filtre.IdPublicClient is not null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                requete = requete.Where(x => x.IdClientNavigation.IdPublic == _filtre.IdPublicClient);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            requete = requete.Where(x =>
                x.DatePourLe.Date >= _filtre.DateDebut.ToDateTime(TimeOnly.MinValue) &&
                x.DatePourLe.Date <= _filtre.DateFin.ToDateTime(TimeOnly.MinValue)
            );
        }

        int total = await requete.CountAsync();

        var liste = await requete 
            .Paginer(_filtre.NumPage, _filtre.NbParPage)
            .Select(x => new CommandeExport
            {
                Numero = x.Numero,
                Date = x.DatePourLe,
                EstLivraison = x.EstLivraison,
                Status = _filtre.Status == EStatusCommande.Tout ? x.DateValidation.HasValue ? EStatusCommande.Valider : x.DateAnnulation.HasValue ? EStatusCommande.Annuler : EStatusCommande.EnAttenteValidation : _filtre.Status,
            
                Client = x.IdClientNavigation != null ? new CommandeClientExport
                {
                    IdPublic = x.IdClientNavigation.IdPublic,
                    Nom = x.IdClientNavigation.Nom,
                    Adresse = x.IdClientNavigation.Adresse
                } : null,

                Livraison = x.IdLivraisonNavigation != null ? new CommandeLivraisonExport
                {
                    IdPublic = x.IdLivraisonNavigation.IdPublic,
                    Ordre = x.OrdreLivraison.GetValueOrDefault()
                } : null,

                ListeProduit = x.ProduitCommandes.Select(y => new CommandeProduitExport
                {
                    IdPublic = y.IdProduitNavigation.IdPublic,
                    Nom = y.IdProduitNavigation.Nom,
                    Quantite = y.Quantite,
                    PrixHT = y.PrixHt,
                    Tva = y.IdProduitNavigation.IdTvaNavigation.Valeur
                }).ToArray()
            })
            .OrderBy(x => x.Date)
            .ToArrayAsync();

        return new PaginationExport<CommandeExport>
        { 
            Liste = liste,
            NumPage = _filtre.NumPage,
            NbParPage = _filtre.NbParPage,
            Total = total
        };
    }

    public async Task<CommandeExport?> InfoAsync(string _numero, int _idGroupe)
    {
        var commande = await _context.Commandes
            .Where(x => x.Numero == _numero && x.IdGroupe == _idGroupe)
            .Select(x => new CommandeExport
            {
                Numero = x.Numero,
                Date = x.DatePourLe,
                EstLivraison = x.EstLivraison,
                Status = x.DateValidation.HasValue ? EStatusCommande.Valider : x.DateAnnulation.HasValue ? EStatusCommande.Annuler : EStatusCommande.EnAttenteValidation,

                Client = x.IdClientNavigation != null ? new CommandeClientExport
                {
                    IdPublic = x.IdClientNavigation.IdPublic,
                    Nom = x.IdClientNavigation.Nom,
                    Adresse = x.IdClientNavigation.Adresse
                } : null,

                Livraison = x.IdLivraisonNavigation != null ? new CommandeLivraisonExport
                {
                    IdPublic = x.IdLivraisonNavigation.IdPublic,
                    Ordre = x.OrdreLivraison.GetValueOrDefault()
                } : null,

                ListeProduit = x.ProduitCommandes.Select(y => new CommandeProduitExport
                {
                    IdPublic = y.IdProduitNavigation.IdPublic,
                    Nom = y.IdProduitNavigation.Nom,
                    Quantite = y.Quantite,
                    PrixHT = y.PrixHt,
                    Tva = y.IdProduitNavigation.IdTvaNavigation.Valeur
                }).ToArray()
            })
            .FirstOrDefaultAsync();

        return commande;
    }

    public async Task<bool> AjouterAsync(Commande _commande, ProduitCommandeImport[] _listeProduitCommande)
    {
        _context.Commandes.Add(_commande);
        int nb = await _context.SaveChangesAsync();

        if (nb == 0)
            return false;

        List<ProduitCommande> listeProduitCommande = await FormaterListeProduitCommandeAsync(_commande.Id, _listeProduitCommande);

        foreach (var element in listeProduitCommande)
            _commande.PrixTotalHt += element.Quantite * element.Quantite;

        _commande.ProduitCommandes = listeProduitCommande;

        nb = await _context.SaveChangesAsync();

        if (nb == 0)
        {
            await _context.Commandes.Where(x => x.Id == _commande.Id).ExecuteDeleteAsync();
            return false;
        }

        return true;
    }

    public async Task<bool> ModifierAsync(string _numero, CommandeImport _commande)
    {
        var commande = await _context.Commandes.Where(x => x.Numero == _numero).FirstOrDefaultAsync();

        if (commande is null)
            return false;

        decimal totalPrixTotalHt = 0;
        int idClient = await _context.Clients
            .Where(x => x.IdPublic == _commande.IdPublicClient)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        commande.IdClient = idClient == 0 ? null : idClient;
        commande.EstLivraison = _commande.EstLivraison;
        commande.DatePourLe = _commande.Date.ToDateTime(TimeOnly.MinValue);

        await _context.ProduitCommandes.Where(x => x.IdCommande == commande.Id).ExecuteDeleteAsync();

        var listeProduitCommande = await FormaterListeProduitCommandeAsync(commande.Id, _commande.ListeProduit);

        commande.ProduitCommandes = listeProduitCommande;

        foreach (var element in listeProduitCommande)
            totalPrixTotalHt += element.Quantite * element.Quantite;

        commande.PrixTotalHt = totalPrixTotalHt;

        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierStatusAsync(string _numero, EStatusCommandeModifier _status, int _idGroupe)
    {
        if (string.IsNullOrWhiteSpace(_numero))
            return false;

        SetPropertyBuilder<Commande> builder = new();

        switch (_status)
        {
            case EStatusCommandeModifier.Valider:
                builder.SetProperty(x => x.DateValidation, DateTime.UtcNow)
                    .SetProperty(x => x.DateAnnulation, value: null);
                break;

            case EStatusCommandeModifier.Annuler:
                builder.SetProperty(x => x.DateAnnulation, DateTime.UtcNow)
                    .SetProperty(x => x.DateValidation, value: null);
                break;

            case EStatusCommandeModifier.Livrer:
                builder.SetProperty(x => x.DatLivraison, DateTime.UtcNow)
                    .SetProperty(x => x.DateAnnulation, value: null);
                break;
        }

        if(builder.SetPropertyCalls.Parameters.Count == 0) 
            return false;

        var requete = _context.Commandes.Where(x => x.Numero == _numero && x.IdGroupe == _idGroupe);

        if (_status == EStatusCommandeModifier.Livrer)
            requete = requete.Where(x => x.EstLivraison);

        int nb = await requete.ExecuteUpdateAsync(builder.SetPropertyCalls);

        return nb > 0;
    }

    public async Task<EReponseSupprimerCommande> SupprimerAsync(string _numero, int _idGroupe)
    {
        var commande = await InfoAsync(_numero, _idGroupe);

        if (commande is null)
            return EReponseSupprimerCommande.ExistePas;

        if (commande.Date <= DateTime.UtcNow)
            return EReponseSupprimerCommande.PeutPasEtreSupprimer;

            int total = await _context.Commandes
                .Where(x => x.IdGroupe == _idGroupe && x.Numero == _numero)
                .ExecuteDeleteAsync();

        return total > 0 ? EReponseSupprimerCommande.Ok : EReponseSupprimerCommande.ExistePas;
    }

    public async Task<bool> ExisteAsync(string _numero, int _idGroupe)
    {
        return await _context.Commandes.AnyAsync(x => x.Numero == _numero && x.IdGroupe == _idGroupe);
    }

    private async Task<List<ProduitCommande>> FormaterListeProduitCommandeAsync(
        int _idCommande, 
        ProduitCommandeImport[] _listeProduitCommande
    )
    {
        List<ProduitCommande> listeProduitCommande = [];

        // construction des where SQL pour recuperer les produits
        var predicat = PredicateBuilder.False<Produit>();

        for (int i = 0; i < _listeProduitCommande.Length; i++)
        {
            var element = _listeProduitCommande[i];

            if (element.IdPublic != Guid.Empty)
                predicat = predicat.Or(x => x.IdPublic == element.IdPublic);
        }

        ProduitPrix[] produit = await _context.Produits
            .Where(predicat)
            .Select(x => new ProduitPrix(x.IdPublic, x.Id, x.PrixHt))
            .ToArrayAsync();

        // ajout des produits dans la commande
        for (int i = 0; i < produit.Length; i++)
        {
            var element = produit[i];

            var quantite = _listeProduitCommande
                .First(x => x.IdPublic == element.IdPublic)
                .Quantite;

            listeProduitCommande.Add(new ProduitCommande
            {
                IdCommande = _idCommande,
                IdProduit = element.Id,
                Quantite = quantite,
                PrixHt = element.PrixHt
            });
        }

        return listeProduitCommande;
    }
}
