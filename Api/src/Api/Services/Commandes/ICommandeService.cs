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
    /// Ajouter une commande
    /// </summary>
    /// <param name="_commande">infos de la comamande</param>
    /// <param name="_listeProduitCommande">liste des produits de la commande</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> AjouterAsync(Commande _commande, ProduitCommandeImport[] _listeProduitCommande);

    /// <summary>
    /// Modifier le status d'une commande
    /// </summary>
    /// <param name="_numero">numero de la commande</param>
    /// <param name="_status">nouveau status de la commande</param>
    /// <param name="_idGroupe">id groupe conserner</param>
    /// <returns><see langword="true"/> si ok, sinon <see langword="false"/></returns>
    Task<bool> ModifierStatusAsync(string _numero, EStatusCommandeModifier _status, int _idGroupe);
}
