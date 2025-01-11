using Api.Models;
using Api.ModelsExports.Recettes;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Recettes;

public sealed class RecetteService(BoulangerieContext _context) : IRecetteService
{
    public async Task<RecetteExport[]> ListerAsync(Guid _idPublicProduit, int _idGroupe)
    {
        if (_idPublicProduit == Guid.Empty)
            return [];

        var liste = await _context.Recettes
            .Where(x => x.IdProduitNavigation.IdPublic == _idPublicProduit)
            .Select(x => new RecetteExport
            {
                IdPublicIngredient = x.IdIngredientNavigation.IdPublic,
                IdPublicProduit = x.IdProduitNavigation.IdPublic,
                NomProduit = x.IdProduitNavigation.Nom,
                NomIngredient = x.IdIngredientNavigation.Nom,
                Quantite = x.Quantite
            })
            .ToArrayAsync();

        return liste;
    }

    public async Task<bool> AjouterAsync(Recette _recette)
    {
        _context.Recettes.Add(_recette);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierQteAsync(
        Guid _idPublicProduit,
        Guid _idPublicIngredient,
        decimal _quantite,
        int _idGroupe
    )
    {
        int nb = 0;

        if(_idPublicIngredient != Guid.Empty && _idPublicProduit != Guid.Empty)
        {
            nb = await _context.Recettes.Where(x =>
                x.IdIngredientNavigation.IdPublic == _idPublicIngredient &&
                x.IdProduitNavigation.IdPublic == _idPublicProduit &&
                x.IdProduitNavigation.IdGroupe == _idGroupe &&
                x.IdIngredientNavigation.IdGroupe == _idGroupe
            )
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quantite, _quantite));
        }

        return nb > 0;
    }

    public async Task<bool> SupprimerAsync(Guid _idPublicProduit, Guid _idPublicIngredient, int _idGroupe)
    {
        if (_idPublicIngredient == Guid.Empty || _idPublicProduit == Guid.Empty)
            return false;

        int nb = await _context.Recettes.Where(x =>
            x.IdIngredientNavigation.IdPublic == _idPublicIngredient &&
            x.IdProduitNavigation.IdPublic == _idPublicProduit &&
            x.IdProduitNavigation.IdGroupe == _idGroupe &&
            x.IdIngredientNavigation.IdGroupe == _idGroupe
        ).ExecuteDeleteAsync();

        return nb > 0;
    }

    public async Task<bool> ExisteAsync(Guid _idPublicProduit, Guid _idPublicIngredient, int _idGroupe)
    {
        if (_idPublicIngredient == Guid.Empty || _idPublicProduit == Guid.Empty)
            return false;

        return await _context.Recettes.AnyAsync(x =>
            x.IdIngredientNavigation.IdPublic == _idPublicIngredient &&
            x.IdProduitNavigation.IdPublic == _idPublicProduit &&
            x.IdProduitNavigation.IdGroupe == _idGroupe &&
            x.IdIngredientNavigation.IdGroupe == _idGroupe
        );
    }
}