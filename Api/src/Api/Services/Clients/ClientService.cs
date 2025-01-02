using Api.Models;
using Api.ModelsExports.Clients;
using Api.ModelsExports;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Clients;

public sealed class ClientService(BoulangerieContext _context): IClientService
{
    public async Task<PaginationExport<ClientExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Clients.Where(x => x.IdGroupe == _idGroupe);

        if (_pagination.ThermeRecherche is not null)
        {
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche)
            );
        }

        requete = requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new ClientExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom,
                Adresse = x.Adresse,
                AdresseFacturation = x.AdresseFacturation,
                InfoComplementaire = x.InfoComplementaire,
                Mail = x.Mail,
                Telephone = x.Telephone
            }).ToArrayAsync();

        PaginationExport<ClientExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<PaginationExport<ClientLegerExport>> ListerLegerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Clients.Where(x => x.IdGroupe == _idGroupe);

        if (_pagination.ThermeRecherche is not null)
        {
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche)
            );
        }

        requete = requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new ClientLegerExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom
            }).ToArrayAsync();

        PaginationExport<ClientLegerExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<int?> RecupererIdAsync(string _idPublicClient, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicClient, out Guid idPublic))
            return await _context.Clients.Where(x => x.IdPublic == idPublic && x.IdGroupe == _idGroupe).Select(x => x.Id).FirstOrDefaultAsync();

        return null;
    }

    public async Task<bool> ExisteAsync(string _idPublicClient, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicClient, out Guid idPublic))
            return await _context.Clients.AnyAsync(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic);

        return false;
    }
}
