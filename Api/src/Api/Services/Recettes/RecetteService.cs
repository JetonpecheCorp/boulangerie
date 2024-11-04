using Api.Models;
using Api.ModelsExports.Recettes;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Recettes;

public sealed class RecetteService(BoulangerieContext _context) : IRecetteService
{
    public async Task<RecetteExport[]> ListerAsync(string _idPublicProduit, int _idGroupe)
    {
        bool ok = Guid.TryParse(_idPublicProduit, out Guid idPublicProduit);

        if (!ok)
            return [];

        var liste = await _context.Recettes
            .Where(x => x.IdProduitNavigation.IdPublic == idPublicProduit)
            .Select(x => new RecetteExport
            {
                IdPublicIngredient = x.IdIngredientNavigation.IdPublic.ToString("D"),
                IdPublicProduit = x.IdProduitNavigation.IdPublic.ToString("D"),
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
}
