
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Groupes;

public sealed class GroupeService(BoulangerieContext _context) : IGroupeService
{
    public async Task<bool> ExisteAsync(int _idGroupe)
    {
        if (_idGroupe <= 0)
            return false;

        return await _context.Groupes.AnyAsync(x => x.Id == _idGroupe);
    }
}
