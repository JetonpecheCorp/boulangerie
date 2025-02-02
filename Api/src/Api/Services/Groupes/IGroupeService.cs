using Api.Models;
using Api.ModelsExports.Groupes;

namespace Api.Services.Groupes;

public interface IGroupeService
{
    /// <summary>
    /// Lister les groupes
    /// </summary>
    /// <returns>
    ///     Tableau des groupes
    /// </returns>
    Task<GroupeExport[]> ListerAsync();

    /// <summary>
    /// Recuperer un groupe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<GroupeExport?> InfoAsync(int _idGroupe);

    /// <summary>
    /// Ajouter un nouveau groupe
    /// </summary>
    /// <param name="_groupe">info du groupe</param>
    /// <returns>
    /// <see langword="true"/> si ajouté, sinon <see langword="false"/>
    /// </returns>
    Task<bool> AjouterAsync(Groupe _groupe);

    /// <summary>
    /// Recuperer le préfix d'un groupe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="string"/> si existe, sinon <see langword="null"/></returns>
    Task<string> PrefixAsync(int _idGroupe);

    /// <summary>
    /// Verifier qu'un groupe existe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>
    /// <see langword="true"/> si existe, sinon <see langword="false"/>
    /// </returns>
    Task<bool> ExisteAsync(int _idGroupe);
}
