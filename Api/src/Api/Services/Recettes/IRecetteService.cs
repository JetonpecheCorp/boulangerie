using Api.Models;

namespace Api.Services.Recettes;

public interface IRecetteService
{
    /// <summary>
    /// Ajouter une recette
    /// </summary>
    /// <param name="_recette">recette conserné</param>
    /// <returns></returns>
    Task<bool> AjouterAsync(Recette _recette);
}
