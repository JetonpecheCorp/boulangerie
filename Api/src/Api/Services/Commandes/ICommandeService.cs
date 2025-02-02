using Api.Enums;
using Api.Models;
using Api.ModelsExports.Commandes;
using Api.ModelsImports.Commandes;

namespace Api.Services.Commandes;

public interface ICommandeService
{
    /// <summary>
    /// Lister les commandes
    /// </summary>
    /// <param name="_filtre"></param>
    /// <param name="_idGroupe">groupe conserné</param>
    /// <returns></returns>
    Task<CommandeExport[]> ListerAsync(CommandeFiltreImport _filtre, int _idGroupe);

    /// <summary>
    /// Recuperer une commande
    /// </summary>
    /// <param name="_numero">numero de la commande conserné</param>
    /// <param name="_idGroupe">id groupe de la commande conserné</param>
    /// <returns></returns>
    Task<CommandeExport?> InfoAsync(string _numero, int _idGroupe);

    /// <summary>
    /// Ajouter une commande
    /// </summary>
    /// <param name="_commande">infos de la comamande</param>
    /// <param name="_listeProduitCommande">liste des produits de la commande</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Commande _commande, ProduitCommandeImport[] _listeProduitCommande);

    /// <summary>
    /// Modifier une commande
    /// </summary>
    /// <param name="_numero"></param>
    /// <param name="_commande"></param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> ModifierAsync(string _numero, CommandeImport _commande);

    /// <summary>
    /// Modifier le status d'une commande
    /// </summary>
    /// <param name="_numero">numero de la commande</param>
    /// <param name="_status">nouveau status de la commande</param>
    /// <param name="_idGroupe">id groupe conserner</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> ModifierStatusAsync(string _numero, EStatusCommandeModifier _status, int _idGroupe);

    /// <summary>
    /// Verifier si une commande existe
    /// </summary>
    /// <param name="_numero">numero de la commande conserné</param>
    /// <param name="_idGroupe">id groupe conserné</param>
    /// <returns><see langword="true"/> si existe, sinon <see langword="false"/></returns>
    Task<bool> ExisteAsync(string _numero, int _idGroupe);
}
