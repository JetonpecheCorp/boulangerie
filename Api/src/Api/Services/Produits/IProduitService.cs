using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Produits;
using Api.ModelsImports;

namespace Api.Services.Produits;

public interface IProduitService
{
    /// <summary>
    /// Lister et paginer les produits
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<ProduitExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Lister et paginer les produits
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<ProduitLegerExport>> ListerLegerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Recuperer id produit
    /// </summary>
    /// <param name="_idPublicProduit">id public de id produit conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>id produit, sinon 0</returns>
    Task<int> RecupererIdAsync(Guid _idPublicProduit, int _idGroupe);

    /// <summary>
    /// Ajouter une nouveau produit
    /// </summary>
    /// <param name="_produit">infos du produit</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Produit _produit);

    /// <summary>
    /// Modifier les infos d'un produit
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_idPublic">id public du produit</param>
    /// <param name="_builder">les infos modifier</param>
    /// <returns><see langword="true"/> si modifié, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(int _idGroupe, Guid _idPublic, SetPropertyBuilder<Produit> _builder);

    /// <summary>
    /// Supprimer un produit soft si utilisé sinon en dur
    /// </summary>
    /// <param name="_idPublic">id public du produit conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> SupprimerAsync(Guid _idPublic, int _idGroupe);

    /// <summary>
    /// Verifier que le produit existe dans le groupe
    /// </summary>
    /// <param name="_idPublicProduit">id public du produit</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublicProduit, int _idGroupe);
}
