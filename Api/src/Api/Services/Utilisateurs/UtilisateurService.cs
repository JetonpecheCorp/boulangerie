using Api.Models;
using Api.ModelsExports.Utilisateurs;
using Api.ModelsExports;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;
using Api.Extensions;

namespace Api.Services.Utilisateurs;

public class UtilisateurService(BoulangerieContext _context) : IUtilisateurService
{
    public async Task<PaginationExport<UtilisateurExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Utilisateurs.Where(x => x.IdGroupe == _idGroupe);

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
            .Select(x => new UtilisateurExport
            {
                IdPublic = x.IdPublic,
                EstAdmin = x.EstAdmin,
                Mail = x.Mail,
                Nom = x.Nom,
                Prenom = x.Prenom,
                Telephone = x.Telephone
            }).ToArrayAsync();

        PaginationExport<UtilisateurExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<PaginationExport<UtilisateurLegerExport>> ListerLegerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Utilisateurs.Where(x => x.IdGroupe == _idGroupe);

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
            .Select(x => new UtilisateurLegerExport
            {
                IdPublic = x.IdPublic,
                NomComplet = x.Prenom + " " + x.Nom,
            }).ToArrayAsync();

        PaginationExport<UtilisateurLegerExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task AjouterAsync(Utilisateur _utilisateur)
    {
        _context.Add(_utilisateur);
        await _context.SaveChangesAsync();
    }

    public async Task AjouterAsync(IReadOnlyList<Utilisateur> _listeUtilisateur)
    {
        _context.AddRange(_listeUtilisateur);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ModifierAsync(SetPropertyBuilder<Utilisateur> _builder, int _idGroupe, Guid _idPublicUtilisateur)
    {
        if (_idPublicUtilisateur == Guid.Empty)
            return false;

        int nb = await _context.Utilisateurs.Where(x => x.IdPublic == _idPublicUtilisateur && x.IdGroupe == _idGroupe)
            .ExecuteUpdateAsync(_builder.SetPropertyCalls);

        return nb > 0;
    }

    public async Task<Utilisateur?> InfoAsync(string _mail)
    {
        return await _context.Utilisateurs.Where(x => x.Mail == _mail).FirstOrDefaultAsync();
    }

    public async Task<int> RecupererId(Guid _idPublic, int _idGroupe)
    {
        return await _context.Utilisateurs
            .Where(x => x.IdPublic == _idPublic && x.IdGroupe == _idGroupe)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExisteAsync(Guid _idPublic, int _idGroupe)
    {
        return await _context.Utilisateurs.AnyAsync(x => x.IdPublic == _idPublic && x.IdGroupe == _idGroupe);
    }

    public async Task<bool> MailExisteAsync(string _mail, Guid? _idPublicUtilisateur = null)
    {
        if(string.IsNullOrWhiteSpace(_mail))
            return false;

        var requete = _context.Utilisateurs.Where(x => x.Mail == _mail);

        if(_idPublicUtilisateur is not null && _idPublicUtilisateur != Guid.Empty)
            requete = requete.Where(x => x.IdPublic != _idPublicUtilisateur);

        return await requete.AnyAsync();
    }
}
