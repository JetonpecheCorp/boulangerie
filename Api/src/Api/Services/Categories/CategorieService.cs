using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Categories;

public class CategorieService(BoulangerieContext _context): ICategorieService
{
    public async Task<int> RecupererIdAsync(string _idPublicCategorie, int _idGroupe)
    {
        if (Guid.TryParse(_idPublicCategorie, out Guid idPublic))
        {
            return await _context.Categories
                .Where(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        return 0;
    }

    public async Task<bool> ExisteAsync(string _idPublicCategorie, int _idGroupe)
    {
        if(Guid.TryParse(_idPublicCategorie, out Guid idPublic))
            return await _context.Categories.AnyAsync(x => x.IdGroupe == _idGroupe && x.IdPublic == idPublic);

        return false;
    }
}
