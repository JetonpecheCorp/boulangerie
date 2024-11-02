using Api.ModelsExports.Tvas;

namespace Api.Services.Tvas;

public interface ITvaService
{
    /// <summary>
    /// Lister les TVA
    /// </summary>
    /// <returns></returns>
    Task<TvaExport[]> ListerAsync();

    /// <summary>
    /// Vérifier que la TVA exite
    /// </summary>
    /// <param name="_idTva">id tva a verifier</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(int _idTva);
}
