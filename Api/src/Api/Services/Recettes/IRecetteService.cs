using Api.Models;
using Api.ModelsExports.Recettes;

namespace Api.Services.Recettes;

public interface IRecetteService
{
    /// <summary>
    /// Lister la recette d'un produit
    /// </summary>
    /// <param name="_idPublicProduit">id public du produit conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<RecetteExport[]> ListerAsync(string _idPublicProduit, int _idGroupe);

    /// <summary>
    /// Ajouter une recette
    /// </summary>
    /// <param name="_recette">recette conserné</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Recette _recette);
}
