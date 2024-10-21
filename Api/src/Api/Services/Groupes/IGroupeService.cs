namespace Api.Services.Groupes;

public interface IGroupeService
{
    /// <summary>
    /// Verifier qu'un groupe existe
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns>
    /// <see langword="true"/> si existe, sinon <see langword="false"/>
    /// </returns>
    Task<bool> ExisteAsync(int _idGroupe);
}
