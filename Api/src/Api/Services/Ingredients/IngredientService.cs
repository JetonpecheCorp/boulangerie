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
            .Skip((_pagination.NumPage - 1) * _pagination.NbParPage)
            .Take(_pagination.NbParPage)
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

    public async Task<bool> AjouterAsync(Ingredient _ingredient)
    {
        _context.Add(_ingredient);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ModifierAsync(int _idGroupe, string _idPublic, SetPropertyBuilder<Ingredient> _builder)
    {
        int nb = 0;
        
        if(Guid.TryParse(_idPublic, out Guid idPublic))
        {
            nb = await _context.Ingredients
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic)
                .ExecuteUpdateAsync(_builder.SetPropertyCalls);
        }

        return nb > 0;
    }
}
