using Api.Models;
using Api.ModelsExports.Clients;
using Api.ModelsExports;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;
using Api.Extensions;

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
            .Paginer(_pagination.NumPage, _pagination.NbParPage)
            .Select(x => new ClientExport
            {
                IdPublic = x.IdPublic,
                Nom = x.Nom,
                Adresse = x.Adresse,
                AdresseFacturation = x.AdresseFacturation,
                InfoComplementaire = x.InfoComplementaire,
                Mail = x.Mail,
                Telephone = x.Telephone,
                ConnexionBloquer = x.ConnexionBloquer,
                PossedeCompte = x.Login != null && x.Mdp != null
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
            .Paginer(_pagination.NumPage, _pagination.NbParPage)
            .Select(x => new ClientLegerExport
            {
                IdPublic = x.IdPublic,
                Nom = x.Nom,
                Adresse = x.Adresse
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

    public async Task<int> RecupererIdAsync(Guid _idPublicClient, int _idGroupe)
    {
        if (_idPublicClient == Guid.Empty)
            return 0;

        return await _context.Clients.Where(x => x.IdPublic == _idPublicClient && x.IdGroupe == _idGroupe)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task AjouterAsync(Client _client)
    {
        _context.Add(_client);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ModifierAsync(SetPropertyBuilder<Client> _builder, int _idGroupe, Guid _idPublicClient)
    {
        if (_idPublicClient == Guid.Empty)
            return false;

        int nb = await _context.Clients
            .Where(x => x.IdPublic == _idPublicClient && x.IdGroupe == _idGroupe)
            .ExecuteUpdateAsync(_builder.SetPropertyCalls);

        return nb > 0;
    }

    public async Task<bool> ExisteAsync(Guid _idPublicClient, int _idGroupe)
    {
        return await _context.Clients.AnyAsync(x => x.IdGroupe == _idGroupe && x.IdPublic == _idPublicClient);
    }
}
