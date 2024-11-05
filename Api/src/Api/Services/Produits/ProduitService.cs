using Api.Models;
using Api.ModelsExports.Produits;
using Api.ModelsExports;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;
using Api.Extensions;

namespace Api.Services.Produits;

public sealed class ProduitService(BoulangerieContext _context) : IProduitService
{
    public async Task<PaginationExport<ProduitExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Produits.Where(x => x.IdGroupe == _idGroupe);

        if (_pagination.ThermeRecherche is not null)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche) || x.CodeInterne.Contains(_pagination.ThermeRecherche)
            );
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        requete = requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new ProduitExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom,
                CodeInterne = x.CodeInterne,
                Stock = x.Stock,
                StockAlert = x.StockAlert,
                Poids = x.Poids,
                PrixHt = x.PrixHt,
                Allergene = x.Alergene,
                Tva = new()
                {
                    Id = x.IdTva,
                    Valeur = x.IdTvaNavigation.Valeur
                },
                Categorie = new()
                {
                    IdPublic = x.IdCategorieNavigation.IdPublic.ToString("D"),
                    Nom = x.IdCategorieNavigation.Nom
                }

            }).ToArrayAsync();

        for (int i = 0; i < liste.Length; i++)
        {
            var element = liste[i];

            if (string.IsNullOrWhiteSpace(element.Allergene))
                continue;

            element.ListeAllergene = element.Allergene.Split(',');
        }

        PaginationExport<ProduitExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<int> RecupererIdAsync(string _idPublicProduit, int _idGroupe)
    {
        int id = 0;

        if (Guid.TryParse(_idPublicProduit, out Guid idPublicProduit))
        {
            id = await _context.Produits.Where(x => x.IdPublic == idPublicProduit && x.IdGroupe == _idGroupe)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
        }

        return id;
    }

    public async Task<bool> AjouterAsync(Produit _produit)
    {
        _context.Produits.Add(_produit);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierAsync(int _idGroupe, string _idPublic, SetPropertyBuilder<Produit> _builder)
    {
        int nb = 0;

        if(Guid.TryParse(_idPublic, out Guid idPublic))
        {
            nb = await _context.Produits
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic)
                .ExecuteUpdateAsync(_builder.SetPropertyCalls);
        }

        return nb > 0;
    }

    public async Task<bool> ExisteAsync(string _idPublicProduit, int _idGroupe)
    {
        if(Guid.TryParse(_idPublicProduit, out Guid idPublicProduit))
            return await _context.Produits.AnyAsync(x => x.IdPublic == idPublicProduit);

        return false;
    }
}
