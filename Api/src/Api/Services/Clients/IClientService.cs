namespace Api.Services.Clients
{
    public interface IClientService
    {
        /// <summary>
        /// Recuperer Id du client
        /// </summary>
        /// <param name="_idPublicClient">id public conserné</param>
        /// <returns>Id</returns>
        Task<int> RecupererIdAsync(string _idPublicClient);

        /// <summary>
        /// Vérifier que la catégorie exite
        /// </summary>
        /// <param name="_idPublicClient">id public client a vérifier</param>
        /// <param name="_idGroupe">id groupe du client a vérifier</param>
        /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
        Task<bool> ExisteAsync(string _idPublicClient, int _idGroupe);
    }
}
