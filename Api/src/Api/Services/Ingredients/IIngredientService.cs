using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Ingredients;
using Api.ModelsImports;

namespace Api.Services.Ingredients;

public interface IIngredientService
{
    /// <summary>
    /// Lister et paginer les ingredients
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<IngredientExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Ajouter un nouvelle ingredient
    /// </summary>
    /// <param name="_ingredient">ingredient conserné</param>
    /// <returns><see langword="true"/> si ajouté, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Ingredient _ingredient);

    /// <summary>
    /// Modifier les infos d'un ingredient
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_idPublic">id public de l'ingredient</param>
    /// <param name="_builder">les infos modifier</param>
    /// <returns><see langword="true"/> si modifié, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(int _idGroupe, string _idPublic, SetPropertyBuilder<Ingredient> _builder);
}
