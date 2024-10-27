using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Ingredients;

namespace Api.Services.Ingredients;

public interface IIngredientService
{
    /// <summary>
    /// Lister et paginer les ingredients
    /// </summary>
    /// <param name="_numPage">numero de la page conserné</param>
    /// <param name="_nbParPage">nombre de resultat par page</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<IngredientExport>> ListerAsync(int _numPage, int _nbParPage, int _idGroupe);

    /// <summary>
    /// Ajouter un nouvelle ingredient
    /// </summary>
    /// <param name="_ingredient">ingredient conserné</param>
    /// <returns><see langword="true"/> si ajouté, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Ingredient _ingredient);
}
