using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Tvas;

public class TvaService(BoulangerieContext _context): ITvaService
{
    public async Task<bool> ExisteAsync(int _idTva)
    {
        return await _context.Tvas.AnyAsync(x => x.Id == _idTva);
    }
}
