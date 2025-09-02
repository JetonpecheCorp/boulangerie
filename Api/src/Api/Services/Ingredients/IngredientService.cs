using Api.ModelsExports.Ingredients;
using Api.ModelsExports;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Api.ModelsImports;
using Api.Extensions;

namespace Api.Services.Ingredients;

public class IngredientService(BoulangerieContext _context): IIngredientService
{
    public async Task<PaginationExport<IngredientExport>> ListerAsync(PaginationImport _pagination, int _idGroupe)

    {
        var requete = _context.Ingredients
            .Where(x => x.IdGroupe == _idGroupe && !x.EstSupprimer);

        if (_pagination.ThermeRecherche is not null)
        {
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
            requete = requete.Where(x =>
                x.Nom.Contains(_pagination.ThermeRecherche) || x.CodeInterne.Contains(_pagination.ThermeRecherche)
            );
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        }

        requete= requete.OrderBy(x => x.Nom);

        int total = await requete.CountAsync();

        var liste = await requete
            .Paginer(_pagination.NumPage, _pagination.NbParPage)
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
            NumPage = _pagination.NumPage,
            NbParPage = _pagination.NbParPage,
            Total = total,
            Liste = liste
        };

        return pagination;
    }

    public async Task<int> RecupererIdAsync(Guid _idPublicIngredient, int _idGroupe)
    {
        int id = 0;

        if(_idPublicIngredient != Guid.Empty)
        {
            id = await _context.Ingredients.Where(x => x.IdPublic == _idPublicIngredient && x.IdGroupe == _idGroupe)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
        }

        return id;
    }

    public async Task<bool> AjouterAsync(Ingredient _ingredient)
    {
        _context.Add(_ingredient);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierAsync(int _idGroupe, Guid _idPublic, SetPropertyBuilder<Ingredient> _builder)
    {
        int nb = 0;
        
        if(_idPublic != Guid.Empty)
        {
            nb = await _context.Ingredients
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == _idPublic)
                .ExecuteUpdateAsync(_builder.SetPropertyCalls);
        }

        return nb > 0;
    }

    public async Task<bool> SupprimerAsync(Guid _idPublic, int _idGroupe)
    {
        if(_idPublic == Guid.Empty)
            return false;

        int idIngredient = await RecupererIdAsync(_idPublic, _idGroupe);
        int total = 0;

        bool existe = await _context.Recettes.AnyAsync(x => x.IdIngredient == idIngredient) ||
            await _context.Fournisseurs.AnyAsync(x => x.IdIngredients.Any(y => y.Id == idIngredient));

        if (existe)
        {
            total = await _context.Ingredients
                .Where(x => x.Id == idIngredient)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.EstSupprimer, true));
        }
        else
        {
            total = await _context.Ingredients
                .Where(x => x.Id == idIngredient)
                .ExecuteDeleteAsync();
        }

        return total > 0;
    }

    public async Task<bool> ExisteAsync(Guid _idPublicIngredient, int _idGroupe)
    {
        if (_idPublicIngredient == Guid.Empty)
            return false;

        return await _context.Ingredients.AnyAsync(x => x.IdPublic == _idPublicIngredient);
    }
}
