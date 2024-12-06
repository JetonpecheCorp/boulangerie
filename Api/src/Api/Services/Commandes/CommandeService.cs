using Api.Extensions;
using Api.Models;
using Api.ModelsImports.Commandes;
using Microsoft.EntityFrameworkCore;
using Api.Enums;
using Api.ModelsExports.Commandes;
using ProduitPrix = (System.Guid IdPublic, int Id, decimal PrixHt);

namespace Api.Services.Commandes;

public sealed class CommandeService(BoulangerieContext _context): ICommandeService
{
    public async Task<CommandeExport[]> ListerAsync(CommandeFiltreImport _filtre, int _idGroupe)
    {
        var requete = _context.Commandes
            .Where(x => x.IdClientNavigation.IdGroupe == _idGroupe);

        requete = _filtre.Status switch
        {
            EStatusCommande.Valider => requete.Where(x => x.DateValidation.HasValue),
            EStatusCommande.EnAttenteValidation => requete.Where(x => !x.DateValidation.HasValue && !x.DateAnnulation.HasValue),
            EStatusCommande.Annuler => requete.Where(x => x.DateAnnulation.HasValue),
            EStatusCommande.Livrer => requete.Where(x => x.DatLivraison.HasValue),
            _ => requete
        };

        var liste = await requete.Where(x =>
            x.DatePourLe.Date >= _filtre.DateDebut.ToDateTime(TimeOnly.MinValue) &&
            x.DatePourLe.Date <= _filtre.DateFin.ToDateTime(TimeOnly.MinValue)
        )
        .Select(x => new CommandeExport
        {
            Numero = x.Numero,
            Date = x.DatePourLe,
            EstLivraison = x.EstLivraison,
            
            Client = new CommandeClientExport
            {
                IdPublic = x.IdClientNavigation.IdPublic,
                Nom = x.IdClientNavigation.Nom
            },

            ListeProduit = x.ProduitCommandes.Select(y => new CommandeProduitExport
            {
                IdPublic = y.IdProduitNavigation.IdPublic,
                Nom = y.IdProduitNavigation.Nom,
                Quantite = y.Quantite
            }).ToArray()
        })
        .OrderBy(x => x.Date)
        .ToArrayAsync();

        return liste;
    }

    public async Task<bool> AjouterAsync(Commande _commande, ProduitCommandeImport[] _listeProduitCommande)
    {
        _context.Commandes.Add(_commande);
        int nb = await _context.SaveChangesAsync();

        if (nb == 0)
            return false;

        List<ProduitCommande> listeProduitCommande = [];
        decimal prixTotalHt = 0;

        // construction des where SQL pour recuperer les produits
        var predicat = PredicateBuilder.False<Produit>();

        for (int i = 0; i < _listeProduitCommande.Length; i++)
        {
            var element = _listeProduitCommande[i];

            if(Guid.TryParse(element.IdPublicProduit, out Guid idPublic))
                predicat = predicat.Or(x => x.IdPublic == idPublic);
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
                .First(x => x.IdPublicProduit == element.IdPublic.ToString("D"))
                .Quantite;

            _context.ProduitCommandes.Add(new()
            {
                IdCommande = _commande.Id,
                IdProduit = element.Id,
                Quantite = quantite,
                PrixHt = element.PrixHt
            });

            prixTotalHt += element.PrixHt * quantite;
        }

        _commande.PrixTotalHt = prixTotalHt;

        nb = await _context.SaveChangesAsync();

        if (nb == 0)
        {
            await _context.Commandes.Where(x => x.Id == _commande.Id).ExecuteDeleteAsync();
            return false;
        }

        return true;
    }
}
