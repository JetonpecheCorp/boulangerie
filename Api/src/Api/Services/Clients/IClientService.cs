using Api.ModelsExports;
using Api.ModelsExports.Clients;
using Api.ModelsImports;

namespace Api.Services.Clients
{
    public interface IClientService
    {
        /// <summary>
        /// Lister et paginer les clients
        /// </summary>
        /// <param name="_pagination">pagination</param>
        /// <param name="_idGroupe">id groupe conserné</param>
        /// <returns></returns>
        Task<PaginationExport<ClientExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

        /// <summary>
        /// Lister et paginer les clients
        /// </summary>
        /// <param name="_pagination">pagination</param>
        /// <param name="_idGroupe">id groupe conserné</param>
        /// <returns></returns>
        Task<PaginationExport<ClientLegerExport>> ListerLegerAsync(PaginationImport _pagination, int _idGroupe);

        /// <summary>
        /// Recuperer Id du client
        /// </summary>
        /// <param name="_idPublicClient">id public conserné</param>
        /// <returns><see langword="int"/>id, sinon <see langword="null"/></returns>
        Task<int?> RecupererIdAsync(string _idPublicClient);

        /// <summary>
        /// Vérifier que la catégorie exite
        /// </summary>
        /// <param name="_idPublicClient">id public client a vérifier</param>
        /// <param name="_idGroupe">id groupe du client a vérifier</param>
        /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
        Task<bool> ExisteAsync(string _idPublicClient, int _idGroupe);
    }
}
