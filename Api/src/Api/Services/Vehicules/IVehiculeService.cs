using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Vehicules;
using Api.ModelsImports;

namespace Api.Services.Vehicules;

public interface IVehiculeService
{
    /// <summary>
    /// Lister et paginer les véhicules
    /// </summary>
    /// <param name="_pagination"></param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<VehiculeExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Recuperer id vehicule
    /// </summary>
    /// <param name="_idPublic">id public vehicule conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<int> RecupererId(Guid _idPublic, int _idGroupe);

    /// <summary>
    /// Ajouter un vehicule
    /// </summary>
    /// <param name="_vehicule">info du vehicule</param>
    /// <returns><see langword="true"/> si ajouté, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Vehicule _vehicule);

    /// <summary>
    /// Modifier un vehicule
    /// </summary>
    /// <param name="_nom">nouveau nom</param>
    /// <param name="_immatriculation">nouvelle immatriculation</param>
    /// <param name="_infoSup">nouvelles infos</param>
    /// <param name="_idPublicVehicule">id public vehicule a modifier</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si modifié, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(string _nom, string _immatriculation, string? _infoSup, string _idPublicVehicule, int _idGroupe);

    /// <summary>
    /// Supprimer un vehicule soft si utilisée sinon en dur
    /// </summary>
    /// <param name="_idPublic">id public véhicule a supprimer</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si supprimé, sinon <see langword="false"/></returns>
    Task<bool> SupprimerAsync(Guid _idPublic, int _idGroupe);

    /// <summary>
    /// Verifier si un vehicule existe
    /// </summary>
    /// <param name="_idPublic">id public du vehicule</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(Guid _idPublic, int _idGroupe);
}
