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
    /// Recuperer id ingredient
    /// </summary>
    /// <param name="_idPublicIngredient">id public de id ingredient conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>id ingredient, sinon 0</returns>
    Task<int> RecupererIdAsync(Guid _idPublicIngredient, int _idGroupe);

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
    Task<bool> ModifierAsync(int _idGroupe, Guid _idPublic, SetPropertyBuilder<Ingredient> _builder);

    /// <summary>
    /// Verifier que l'ingredient existe dans le groupe
    /// </summary>
    /// <param name="_idPublicIngredient">id public du produit</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublicIngredient, int _idGroupe);
}
