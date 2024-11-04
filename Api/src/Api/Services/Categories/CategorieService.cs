using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Categories;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Categories;

public class CategorieService(BoulangerieContext _context): ICategorieService
{
    public async Task<CategorieExport[]> ListerAsync(int _idGroupe)
    {
        var liste = await _context.Categories
            .Where(x => x.IdGroupe == _idGroupe && !x.EstSupprimer)
            .OrderBy(x => x.Nom)
            .Select(x => new CategorieExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom
            }).ToArrayAsync();

        return liste;
    }

    public async Task<PaginationExport<CategorieExport>> ListerAsync(
        PaginationImport _pagination, 
        int _idGroupe
    )
    {
        var requete = _context.Categories
    .Where(x => x.IdGroupe == _idGroupe && !x.EstSupprimer);

        if (_pagination.ThermeRecherche is not null)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche)
            );
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        requete = requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new CategorieExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom
            }).ToArrayAsync();

        PaginationExport<CategorieExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }
    public async Task<int> RecupererIdAsync(string _idPublicCategorie, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicCategorie, out Guid idPublic))
        {
            return await _context.Categories
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        return 0;
    }

    public async Task<Guid> AjouterAsync(string _nom, int _idGroupe)
    {
        Categorie categorie = new()
        {
            Nom = _nom.XSS(),
            IdGroupe = _idGroupe,
            IdPublic = Guid.NewGuid()
        };

        _context.Categories.Add(categorie);
        int nb = await _context.SaveChangesAsync();

        return nb > 0 ? categorie.IdPublic : Guid.Empty;
    }

    public async Task<bool> ModifierAsync(string _nom, string _idPublicCategorie, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicCategorie, out Guid idPublic))
        {

            int nb = await _context.Categories
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.Nom, _nom.XSS()));

            return nb > 0;
        }

        return false;
    }

    public async Task<bool> ExisteAsync(string _idPublicCategorie, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicCategorie, out Guid idPublic))
            return await _context.Categories.AnyAsync(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic);

        return false;
    }
}
