using Api.ModelsExports.Livraisons;
using Api.ModelsExports;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.ModelsImports.Livraisons;

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

        if(_filtre.Date != null)
            requete = requete.Where(x => x.Date == _filtre.Date);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_filtre.NumPage - 1) * _filtre.NbParPage)
            .Take(_filtre.NbParPage)
            .Select(x => new LivraisonExport
            {
                IdPublic = x.IdPublic,
                FraisHT = x.Frais,
                Numero = x.Numero,
                Date = x.Date,
                Vehicule = new LivraisonVehiculeExport 
                { 
                    IdPublic = x.IdVehiculeNavigation.IdPublic,
                    Immatriculation = x.IdVehiculeNavigation.Immatriculation,
                    InfoComplementaire = x.IdVehiculeNavigation.InfoComplementaire
                },
                Conducteur = new LivraisonConducteurExport 
                {
                    Nom = x.IdUtilisateurNavigation.Nom,
                    Prenom = x.IdUtilisateurNavigation.Prenom
                }
            }).ToArrayAsync();

        return new PaginationExport<LivraisonExport>
        {
            Liste = liste,
            Total = total,
            NumPage = _filtre.NumPage,
            NbParPage = _filtre.NbParPage
        };
    }
}
