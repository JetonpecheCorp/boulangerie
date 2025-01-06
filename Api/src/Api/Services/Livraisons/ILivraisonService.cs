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

    Task<int> AjouterAsync(Livraison _livraison, LivraisonCommande[] _listeCommande);
}
