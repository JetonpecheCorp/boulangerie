using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Fournisseurs;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Api.Services.Fournisseurs;

public class FournisseurService(BoulangerieContext _context): IFournisseurService
{
    public async Task<PaginationExport<FournisseurExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)
    {
        var requete = _context.Fournisseurs
            .Where(x => x.IdGroupe == _idGroupe && !x.EstSupprimer);

        if (_pagination.ThermeRecherche is not null)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche) || 
                x.Adresse.Contains(_pagination.ThermeRecherche) || 
                x.Mail.Contains(_pagination.ThermeRecherche) ||
                x.Telephone.Contains(_pagination.ThermeRecherche)
            );
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        requete = requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
            .Select(x => new FournisseurExport
            {
                IdPublic = x.IdPublic.ToString("D"),
                Nom = x.Nom,
                Adresse = x.Adresse,
                Mail = x.Mail,
                Telephone = x.Telephone
            }).ToArrayAsync();

        PaginationExport<FournisseurExport> pagination = new()
        {
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<bool> AjouterAsync(Fournisseur _fournisseur, string[] _listeIdPublicIngredient, string[] _listeIdPublicProduit)
    {
        if (_listeIdPublicIngredient.Length > 0)
        {
            var predicat = PredicateBuilder.False<Ingredient>();

            foreach (var element in _listeIdPublicIngredient)
            {
                var idPublic = Guid.Parse(element);

                predicat = predicat.Or(x => x.IdPublic == idPublic);
            }

            var listeIngredient = await _context.Ingredients.Where(predicat).ToArrayAsync();
            _fournisseur.IdIngredients = listeIngredient;
        }

        if (_listeIdPublicProduit.Length > 0)
        {
            var predicat = PredicateBuilder.False<Produit>();

            foreach (var element in _listeIdPublicProduit)
            {
                var idPublic = Guid.Parse(element);

                predicat = predicat.Or(x => x.IdPublic == idPublic);
            }

            var listeProduit = await _context.Produits.Where(predicat).ToArrayAsync();
            _fournisseur.IdProduits = listeProduit;
        }

        _context.Fournisseurs.Add(_fournisseur);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierAsync(
        int _idGroupe, 
        string _idPublicFournisseur, 
        SetPropertyBuilder<Fournisseur> _builder,
        string[] _listeIdPublicProduit,
        string[] _listeIdPublicIngredient
    )
    {
        int nb = 0;

        if(Guid.TryParse(_idPublicFournisseur, out Guid idPublicFournisseur))
        {
            nb = await _context.Fournisseurs.Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublicFournisseur)
                .ExecuteUpdateAsync(_builder.SetPropertyCalls);

            if (nb == 0)
                return false;
        }

        _context.Database.ExecuteSqlRaw("DELETE fi.* FROM FournisseurIngredient fi JOIN Fournisseur f ON f.Id = fi.IdFournisseur WHERE IdPublic = ?", _idPublicFournisseur);

        if (_listeIdPublicIngredient.Length > 0)
        {
            var predicat = PredicateBuilder.False<Ingredient>();

            foreach (var element in _listeIdPublicIngredient)
            {
                var idPublic = Guid.Parse(element);

                predicat = predicat.Or(x => x.IdPublic == idPublic);
            }

            var listeIngredient = await _context.Ingredients.Where(predicat).ToArrayAsync();

            var listeFournisseurIngredient = _context.Fournisseurs
                .Where(x => x.IdPublic == idPublicFournisseur)
                .First()
                .IdIngredients;

            foreach (var item in listeIngredient)
                listeFournisseurIngredient.Add(item);

           nb = await _context.SaveChangesAsync();
        }

        _context.Database.ExecuteSqlRaw("DELETE fp.* FROM FournisseurProduit fp JOIN Fournisseur f ON f.Id = fp.IdFournisseur WHERE IdPublic = ?", _idPublicFournisseur);

        if (_listeIdPublicProduit.Length > 0)
        {
            var predicat = PredicateBuilder.False<Produit>();

            foreach (var element in _listeIdPublicProduit)
            {
                var idPublic = Guid.Parse(element);

                predicat = predicat.Or(x => x.IdPublic == idPublic);
            }

            var listeProduit = await _context.Produits.Where(predicat).ToArrayAsync();

            var listeFournisseurProduit = _context.Fournisseurs
                .Where(x => x.IdPublic == idPublicFournisseur)
                .First()
                .IdProduits;

            foreach (var item in listeProduit)
                listeFournisseurProduit.Add(item);

            nb = await _context.SaveChangesAsync();
        }

        return nb > 0;
    }

    public async Task<bool> ArchiverAsync(int _idGroupe, string _idPublicFournisseur)
    {
        int nb = 0;

        if (Guid.TryParse(_idPublicFournisseur, out Guid idPublicFournisseur))
        {
            nb = await _context.Fournisseurs.Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublicFournisseur)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.EstSupprimer, true));
        }

        return nb > 0;
    }
}
