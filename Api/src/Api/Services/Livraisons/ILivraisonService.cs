using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Livraisons;
using Api.ModelsImports.Livraisons;

namespace Api.Services.Livraisons;

public interface ILivraisonService
{
    /// <summary>
    /// Lister et paginer les livraisons
    /// </summary>
    /// <param name="_filtre"></param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<LivraisonExport>> ListerAsync(LivraisonFiltreImport _filtre, int _idGroupe);

    /// <summary>
    /// Recuperer le detail d'une livraison
    /// </summary>
    /// <param name="_idPublicLivraison">id public livraison conserné</param>
    /// <returns><see langword="LivraisonDetailExport"/> si ok, sinon <see langword="null"/></returns>
    Task<LivraisonDetailExport?> RecupererDetailAsync(Guid _idPublicLivraison);

    /// <summary>
    /// Ajouter une livraison
    /// </summary>
    /// <param name="_livraison">info livraison (sans les commandes)</param>
    /// <param name="_listeCommande">liste des commandes de la livraison</param>
    /// <returns>id livraison si OK / 0 => erreur</returns>
    Task<int> AjouterAsync(Livraison _livraison, LivraisonCommande[] _listeCommande);
}
