using Api.Extensions;
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
    Task<RecetteExport[]> ListerAsync(Guid _idPublicProduit, int _idGroupe);

    /// <summary>
    /// Ajouter une recette
    /// </summary>
    /// <param name="_recette">recette conserné</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Recette _recette);

    /// <summary>
    /// Modifier une qte
    /// </summary>
    /// <param name="_idPublicProduit">id public produit conserné</param>
    /// <param name="_idPublicIngredient">id public ingredient conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_quantite">nouvelle quantite</param>
    /// <returns><see langword="true"/> si modifié, sinon <see langword="false"/></returns>
    Task<bool> ModifierQteAsync(
        Guid _idPublicProduit, 
        Guid _idPublicIngredient, 
        decimal _quantite,
        int _idGroupe
    );

    /// <summary>
    /// Supprimer un ingredient de la recette du produit
    /// </summary>
    /// <param name="_idPublicProduit">produit conserné</param>
    /// <param name="_idPublicIngredient">ingredient conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si supprimé, sinon <see langword="false"/></returns>
    Task<bool> SupprimerAsync(Guid _idPublicProduit, Guid _idPublicIngredient, int _idGroupe);

    /// <summary>
    /// Verifier que l'ingredient et le produit existe dans la recette
    /// </summary>
    /// <param name="_idPublicProduit">id public du produit</param>
    /// <param name="_idPublicIngredient">id public d l'ingredient</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublicProduit, Guid _idPublicIngredient, int _idGroupe);
}
