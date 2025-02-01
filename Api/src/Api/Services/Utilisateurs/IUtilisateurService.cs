using Api.Models;
using Api.ModelsExports;
using Api.ModelsImports;
using Api.ModelsExports.Utilisateurs;

namespace Api.Services.Utilisateurs;

public interface IUtilisateurService
{
    /// <summary>
    /// Lister et paginer les utilisateurs
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<UtilisateurExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Lister et paginer les utilisateurs
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<UtilisateurLegerExport>> ListerLegerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Recuperer l'utilisateur par son mail
    /// </summary>
    /// <param name="_mail"></param>
    /// <returns></returns>
    Task<Utilisateur?> InfoAsync(string _mail);

    /// <summary>
    /// Recuperer id utilisateur
    /// </summary>
    /// <param name="_idPublic">id public utilisateur conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<int> RecupererId(Guid _idPublic, int _idGroupe);

    /// <summary>
    /// Ajouter une nouvelle utilisateur
    /// </summary>
    /// <param name="_utilisateur"></param>
    /// <returns></returns>
    Task AjouterAsync(Utilisateur _utilisateur);

    /// <summary>
    /// Verifier qu'un mail existe
    /// </summary>
    /// <param name="_mail">mail conserné</param>
    /// <param name="_idGroupe">id groupe du mail conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> MailExisteAsync(string _mail, int _idGroupe = 0);

    /// <summary>
    /// Verifier si un utilisateur existe
    /// </summary>
    /// <param name="_idPublic">id public de l'utilisateur</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublic, int _idGroupe);
}
