using Api.ModelsExports.Ingredients;
using Api.ModelsExports;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Ingredients;

public class IngredientService(BoulangerieContext _context): IIngredientService
{
    public async Task<PaginationExport<IngredientExport>> ListerAsync(int _numPage, int _nbParPage, int _idGroupe)
    {
        var requete = _context.Ingredients.Where(x => x.IdGroupe == _idGroupe && !x.EstSupprimer).OrderBy(x => x.Nom);
        int total = await requete.CountAsync();

        var liste = await requete
            .Skip((_numPage - 1) * _nbParPage)
            .Take(_nbParPage)
            .Select(x => new IngredientExport
            {
               IdPublic = x.IdPublic.ToString("D"),
               Nom = x.Nom,
               CodeInterne = x.CodeInterne,
               Stock = x.Stock,
               StockAlert = x.StockAlert
            }).ToArrayAsync();

        PaginationExport<IngredientExport> pagination = new()
        {
            NumPage = _numPage,
            NbParPage = _nbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<bool> AjouterAsync(Ingredient _ingredient)
    {
        _context.Add(_ingredient);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }
}
