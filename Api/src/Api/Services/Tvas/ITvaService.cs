namespace Api.Services.Tvas;

public interface ITvaService
{
    /// <summary>
    /// Vérifier que la TVA exite
    /// </summary>
    /// <param name="_idTva">id tva a verifier</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(int _idTva);
}
