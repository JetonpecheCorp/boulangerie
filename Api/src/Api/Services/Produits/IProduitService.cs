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
    /// Ajouter une nouveau produit
    /// </summary>
    /// <param name="_produit">infos du produit</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Produit _produit);
}
