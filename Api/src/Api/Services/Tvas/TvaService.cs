using Api.Models;
using Api.ModelsExports.Tvas;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Tvas;

public class TvaService(BoulangerieContext _context): ITvaService
{
    public async Task<TvaExport[]> ListerAsync()
    {
        var liste = await _context.Tvas
            .OrderBy(x => x.Valeur)
            .Select(x => new TvaExport 
            { 
                Id = x.Id,
                Valeur = x.Valeur
            }).ToArrayAsync();

        return liste;
    }

    public async Task<bool> ExisteAsync(int _idTva)
    {
        return await _context.Tvas.AnyAsync(x => x.Id == _idTva);
    }
}
