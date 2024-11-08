using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Vehicules;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Vehicules;

public class VehiculeService(BoulangerieContext _context) : IVehiculeService
{
    public async Task<PaginationExport<VehiculeExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Vehicules.Where(x => x.IdGroupe == _idGroupe);

        if (_pagination.ThermeRecherche is not null)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x =>
                x.Immatriculation.Contains(_pagination.ThermeRecherche) || x.InfoComplementaire.Contains(_pagination.ThermeRecherche)
            );
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        requete = requete.OrderBy(x => x.Immatriculation);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new VehiculeExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Immatriculation = x.Immatriculation,
                InfoComplementaire = x.InfoComplementaire

            }).ToArrayAsync();

        PaginationExport<VehiculeExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<bool> AjouterAsync(Vehicule _vehicule)
    {
        _context.Vehicules.Add(_vehicule);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierAsync(string _immatriculation, string? _infoSup, string _idPublicVehicule, int _idGroupe)
    {
        int nb = 0;

        if(Guid.TryParse(_idPublicVehicule, out Guid idPublicVehicule))
        {
             nb = await _context.Vehicules
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublicVehicule)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.Immatriculation, _immatriculation)
                    .SetProperty(y => y.InfoComplementaire, _infoSup)
                );

            return nb > 0;
        }

        return false;
    }
}
