using Api.Extensions;
using Api.Models;
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
        /// <param name="_idGroupe">id groupe conserné</param>
        /// <returns><see langword="int"/>id, sinon 0</returns>
        Task<int> RecupererIdAsync(Guid _idPublicClient, int _idGroupe);

        /// <summary>
        /// Ajouter un nouveau client
        /// </summary>
        /// <param name="_client">infos nouveau client</param>
        Task AjouterAsync(Client _client);

        /// <summary>
        /// Modifier un client
        /// </summary>
        /// <param name="_builder">info a modifier</param>
        /// <param name="_idGroupe">id groupe de l'utilisateur conserné</param>
        /// <param name="_idPublicClient">id public utilisateur conserné</param>
        /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
        Task<bool> ModifierAsync(SetPropertyBuilder<Client> _builder, int _idGroupe, Guid _idPublicClient);

        /// <summary>
        /// Vérifier que la catégorie exite
        /// </summary>
        /// <param name="_idPublicClient">id public client a vérifier</param>
        /// <param name="_idGroupe">id groupe du client a vérifier</param>
        /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
        Task<bool> ExisteAsync(Guid _idPublicClient, int _idGroupe);
    }
}
