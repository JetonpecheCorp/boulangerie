using Api.Models;

namespace Api.Services.Produits;

public sealed class ProduitService(BoulangerieContext _context) : IProduitService
{
    public async Task<bool> AjouterAsync(Produit _produit)
    {
        _context.Produits.Add(_produit);
        int nb = await _context.SaveChangesAsync();

        return nb > 0;
    }
}
