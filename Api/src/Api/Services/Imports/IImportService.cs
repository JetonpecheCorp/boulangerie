using Api.ModelsExports;

namespace Api.Services.Imports;

public interface IImportService
{
    /// <summary>
    /// Importer des utilisateurs pour un groupe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_fichierCSV">fichier CSV</param>
    /// <returns></returns>
    Task<List<ErreurValidationCSV>> UtilisateurAsync(int _idGroupe, IFormFile _fichierCSV);

    /// <summary>
    /// Importer des ingredients pour un groupe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_fichierCSV">fichier CSV</param>
    /// <returns></returns>
    Task<List<ErreurValidationCSV>> IngredientAsync(int _idGroupe, IFormFile _fichierCSV);
}
