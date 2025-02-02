using Api.ModelsExports.Livraisons;
using Api.ModelsExports;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.ModelsImports.Livraisons;
using Api.ModelsExports.Commandes;
using Api.Extensions;
using LivraisonInfoId = (int Id, System.Guid IdPublicUtilisateur, System.Guid IdPublicVehicule);

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
                        Quantite = z.Quantite,
                        PrixHT = z.PrixHt,
                        Tva = z.IdProduitNavigation.IdTvaNavigation.Valeur
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

    public async Task<bool> ModifierAsync(Guid _idPublicLivraison, int _idGroupe, LivraisonImport _livraison)
    {
        int idUtilisateur;
        int idVehicule;
        SetPropertyBuilder<Livraison> builder = new();

        var livraison = await _context.Livraisons
            .Where(x => x.IdPublic == _idPublicLivraison && x.IdVehiculeNavigation.IdGroupe == _idGroupe)
            .Select(x => new LivraisonInfoId(
                x.Id,
                x.IdUtilisateurNavigation.IdPublic,
                x.IdVehiculeNavigation.IdPublic
            ))
            .FirstOrDefaultAsync();

        if(livraison == default)
            return false;

        if(livraison.IdPublicUtilisateur != _livraison.IdPublicConducteur)
        {
            idUtilisateur = await _context.Utilisateurs
                .Where(x => x.IdPublic == _livraison.IdPublicConducteur)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            builder.SetProperty(x => x.IdUtilisateur, idUtilisateur);
        }

        if(livraison.IdPublicVehicule != _livraison.IdPublicVehicule)
        {
            idVehicule = await _context.Vehicules
               .Where(x => x.IdPublic == _livraison.IdPublicVehicule)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();

            builder.SetProperty(x => x.IdVehicule, idVehicule);
        }

        builder.SetProperty(x => x.Frais, _livraison.Frais);

        await _context.Livraisons.ExecuteUpdateAsync(builder.SetPropertyCalls);

        int nb = await _context.Commandes.Where(x => x.IdLivraison == livraison.Id)
            .ExecuteUpdateAsync(x => 
                x.SetProperty(y => y.IdLivraison, (int?)null)
                .SetProperty(y => y.OrdreLivraison, (int?)null)
            );

        for (int i = 0; i < _livraison.Liste.Length; i++)
        {
            var element = _livraison.Liste[i];

            await _context.Commandes.Where(x => x.Numero == element.Numero)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.OrdreLivraison, element.Ordre)
                    .SetProperty(y => y.IdLivraison, livraison.Id)
                );
        }

        return true;
    }
}
