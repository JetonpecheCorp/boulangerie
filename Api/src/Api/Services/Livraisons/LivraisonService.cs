using Api.ModelsExports.Livraisons;
using Api.ModelsExports;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.ModelsImports.Livraisons;
using Api.ModelsExports.Commandes;

namespace Api.Services.Livraisons;

public sealed class LivraisonService(BoulangerieContext _context): ILivraisonService
{
    public async Task<PaginationExport<LivraisonExport>> ListerAsync(LivraisonFiltreImport _filtre, int _idGroupe)
    {
        var requete = _context.Livraisons.Where(x => x.IdVehiculeNavigation.IdGroupe == _idGroupe);

        if (_filtre.ThermeRecherche is not null)
        {
            requete = requete.Where(x =>
                x.Numero.Contains(_filtre.ThermeRecherche)
            );
        }

        if(_filtre.DateDebut is not null)
            requete = requete.Where(x => x.Date >= _filtre.DateDebut);

        if(_filtre.DateFin is not null)
            requete = requete.Where(x => x.Date <= _filtre.DateFin);

        if (_filtre.IdPublicClient is not null && _filtre.IdPublicClient != Guid.Empty)
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x => x.Commandes.Any(y => y.IdClientNavigation.IdPublic == _filtre.IdPublicClient));
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_filtre.NumPage - 1) * _filtre.NbParPage)
            .Take(_filtre.NbParPage)
            .Select(x => new LivraisonExport
            {
                IdPublic = x.IdPublic,
                FraisHT = x.Frais,
                Numero = x.Numero,
                Date = x.Date
            }).ToArrayAsync();

        return new PaginationExport<LivraisonExport>
        {
            Liste = liste,
            Total = total,
            NumPage = _filtre.NumPage,
            NbParPage = _filtre.NbParPage
        };
    }

    public async Task<LivraisonDetailExport?> RecupererDetailAsync(Guid _idPublicLivraison)
    {
        if (_idPublicLivraison == Guid.Empty)
            return null;

        var detail = await _context.Livraisons
            .Where(x => x.IdPublic == _idPublicLivraison)
            .Select(x => new LivraisonDetailExport
            {
                Conducteur = new LivraisonConducteurExport
                {
                    IdPublic = x.IdPublic,
                    NomComplet = x.IdUtilisateurNavigation.Prenom + " " + x.IdUtilisateurNavigation.Nom
                },

                Vehicule = new LivraisonVehiculeExport
                {
                    IdPublic = x.IdVehiculeNavigation.IdPublic,
                    Immatriculation = x.IdVehiculeNavigation.Immatriculation,
                    Nom = x.IdVehiculeNavigation.Nom
                },

                ListeCommande = x.Commandes.Select(y => new CommandeExport
                {
                    Date = y.DatePourLe,
                    EstLivraison = true,
                    Numero = y.Numero,
                    Status = Enums.EStatusCommande.Valider,
                    Client = y.IdClientNavigation != null ? new CommandeClientExport
                    {
                        Adresse = y.IdClientNavigation.Adresse,
                        IdPublic = y.IdClientNavigation.IdPublic,
                        Nom = y.IdClientNavigation.Nom
                    } : null,

                    ListeProduit = y.ProduitCommandes.Select(z => new CommandeProduitExport
                    {
                        IdPublic = z.IdProduitNavigation.IdPublic,
                        Nom = z.IdProduitNavigation.Nom,
                        Quantite = z.Quantite
                    }).ToArray()
                }).ToArray()
            })
            .FirstOrDefaultAsync();

        return detail;
    }

    public async Task<int> AjouterAsync(Livraison _livraison, LivraisonCommande[] _listeCommande)
    {
        string[] listeNumeroCommande = _listeCommande.Select(x => x.Numero).ToArray();

        Commande[] liste = await _context.Commandes
            .Where(x => listeNumeroCommande.Contains(x.Numero))
            .ToArrayAsync();

        _livraison.Commandes = liste;

        _context.Livraisons.Add(_livraison);
        int nb = await _context.SaveChangesAsync();

        if (nb is 0)
            return 0;

        for (int i = 0; i < liste.Length; i++)
        {
            var element = liste[i];

            int ordre = _listeCommande.First(x => x.Numero == element.Numero).Ordre;

            element.OrdreLivraison = ordre;
        }

        nb = await _context.SaveChangesAsync();

        return _livraison.Id; 
    }
}
