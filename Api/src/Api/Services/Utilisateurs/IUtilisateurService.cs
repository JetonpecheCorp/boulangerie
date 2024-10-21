using Api.Models;

namespace Api.Services.Utilisateurs;

public interface IUtilisateurService
{
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
    Task<bool> MailExiste(string _mail, int _idGroupe = 0);
}
