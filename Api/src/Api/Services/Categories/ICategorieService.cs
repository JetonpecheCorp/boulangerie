using Api.ModelsExports.Categories;

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
    /// Recuperer id cataegorie
    /// </summary>
    /// <param name="_idPublicCategorie">id public de id categorie conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>id categorie, sinon 0</returns>
    Task<int> RecupererIdAsync(string _idPublicCategorie, int _idGroupe);

    /// <summary>
    /// Vérifier que la catégorie exite
    /// </summary>
    /// <param name="_idPublicCategorie">id public categorie a vérifier</param>
    /// <param name="_idGroupe">id groupe de la categorie a vérifier</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(string _idPublicCategorie, int _idGroupe);
}
