using Api.Models;

namespace Api.Services.Produits;

public interface IProduitService
{
    /// <summary>
    /// Ajouter une nouveau produit
    /// </summary>
    /// <param name="_produit">infos du produit</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Produit _produit);
}
