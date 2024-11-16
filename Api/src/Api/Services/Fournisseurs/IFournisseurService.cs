using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Fournisseurs;
using Api.ModelsImports;
using Microsoft.EntityFrameworkCore.Query;

namespace Api.Services.Fournisseurs;

public interface IFournisseurService
{
    /// <summary>
    /// Lister et paginer les fournisseurs
    /// </summary>
    /// <param name="_pagination">pagination</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns></returns>
    Task<PaginationExport<FournisseurExport>> ListerAsync(PaginationImport _pagination, int _idGroupe);

    /// <summary>
    /// Ajouter un nouveau fournisseur
    /// </summary>
    /// <param name="_fournisseur">fournisseur conserné</param>
    /// <returns><see langword="true"/> si ajouté, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Fournisseur _fournisseur);

    /// <summary>
    /// Modifier les infos d'un fournisseur
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_idPublicFournisseur">id public du fournisseur</param>
    /// <param name="_builder">les infos modifier</param>
    /// <returns><see langword="true"/> si modifié, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(int _idGroupe, string _idPublicFournisseur, SetPropertyBuilder<Fournisseur> _builder);

    /// <summary>
    /// Archiver un fournisseur
    /// </summary>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <param name="_idPublicFournisseur">id public fournisseur</param>
    /// <returns></returns>
    Task<bool> ArchiverAsync(int _idGroupe, string _idPublicFournisseur);
}
