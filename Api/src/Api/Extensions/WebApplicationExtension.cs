using Api.Routes;

namespace Api.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication AjouterRouteAPI(this WebApplication _app)
    {
        var apiGroupe = _app.MapGroup("api");

        apiGroupe.MapGroup("utilisateur").AjouterRouteUtilisateur();
        apiGroupe.MapGroup("groupe").AjouterRouteGroupe();
        apiGroupe.MapGroup("authentification").AjouterRouteAuthentification();
        apiGroupe.MapGroup("ingredient").AjouterRouteIngredient();
        apiGroupe.MapGroup("recette").AjouterRouteRecette();
        apiGroupe.MapGroup("produit").AjouterRouteProduit();
        apiGroupe.MapGroup("tva").AjouterRouteTva();
        apiGroupe.MapGroup("categorie").AjouterRouteCategorie();
        apiGroupe.MapGroup("vehicule").AjouterRouteVehicule();
        apiGroupe.MapGroup("fournisseur").AjouterRouteFournisseur();
        apiGroupe.MapGroup("commande").AjouterRouteCommande();
        apiGroupe.MapGroup("client").AjouterRouteClient();
        apiGroupe.MapGroup("livraison").AjouterRouteLivraison();
        apiGroupe.MapGroup("export").AjouterRouteExport();
        apiGroupe.MapGroup("import").AjouterRouteImport();

        return _app;
    }
}
