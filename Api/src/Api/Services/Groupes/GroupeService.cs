using Api.Models;
using Api.ModelsExports.Groupes;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Groupes;

public sealed class GroupeService(BoulangerieContext _context) : IGroupeService
{
    public async Task<GroupeExport[]> ListerAsync()
    {
        return await _context.Groupes.Select(x => new GroupeExport
        {
            Id = x.Id,
            Nom = x.Nom
        }).ToArrayAsync();
    }

    public async Task<string> PrefixAsync(int _idGroupe)
    {
        return await _context.Groupes.Where(x => x.Id == _idGroupe)
            .Select(x => x.Prefix)
            .FirstAsync();
    }

    public async Task<bool> AjouterAsync(Groupe _groupe)
    {
        _context.Add(_groupe);
        var nb = await _context.SaveChangesAsync();

        return nb > 0;
    }

    public async Task<bool> ExisteAsync(int _idGroupe)
    {
        if (_idGroupe <= 0)
            return false;

        return await _context.Groupes.AnyAsync(x => x.Id == _idGroupe);
    }
}
