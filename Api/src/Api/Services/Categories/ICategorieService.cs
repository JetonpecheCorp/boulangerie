using Api.ModelsExports;
using Api.ModelsExports.Categories;
using Api.ModelsImports;

namespace Api.Services.Categories;

public interface ICategorieService
{
    /// <summary>
    /// Lister les categories d'un groupe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<CategorieExport[]> ListerAsync(int _idGroupe);

    /// <summary>
    /// Lister les categories d'un groupe
    /// </summary>
    /// <param name="_pagination">Pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<CategorieExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Recuperer id cataegorie
    /// </summary>
    /// <param name="_idPublicCategorie">id public de id categorie conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>id categorie, sinon 0</returns>
    Task<int> RecupererIdAsync(Guid _idPublicCategorie, int _idGroupe);

    /// <summary>
    /// Ajouter une categorie
    /// </summary>
    /// <param name="_nom">nom de la categorie</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>id public ou vide si erreur</returns>
    Task<Guid> AjouterAsync(string _nom, int _idGroupe);

    /// <summary>
    /// Modifier une categorie
    /// </summary>
    /// <param name="_nom">nouveau nom de la categorie</param>
    /// <param name="_idPublicCategorie">id oublic de la categorie conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(string _nom, Guid _idPublicCategorie, int _idGroupe);

    /// <summary>
    /// Vérifier que la catégorie exite
    /// </summary>
    /// <param name="_idPublicCategorie">id public categorie a vérifier</param>
    /// <param name="_idGroupe">id groupe de la categorie a vérifier</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublicCategorie, int _idGroupe);
}
