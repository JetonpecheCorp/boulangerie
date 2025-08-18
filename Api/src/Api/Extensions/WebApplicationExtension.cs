using Api.Routes;

namespace Api.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication AjouterRouteAPI(this WebApplication _app)
    {
        var apiGroupe = _app.MapGroup("api");

        apiGroupe.MapGroup("authentification").AjouterRouteAuthentification();

        apiGroupe.MapGroup("utilisateur").AjouterRouteUtilisateur().RequireAuthorization();
        apiGroupe.MapGroup("groupe").AjouterRouteGroupe().RequireAuthorization();
        apiGroupe.MapGroup("ingredient").AjouterRouteIngredient().RequireAuthorization();
        apiGroupe.MapGroup("recette").AjouterRouteRecette().RequireAuthorization();
        apiGroupe.MapGroup("produit").AjouterRouteProduit().RequireAuthorization();
        apiGroupe.MapGroup("tva").AjouterRouteTva().RequireAuthorization();
        apiGroupe.MapGroup("categorie").AjouterRouteCategorie().RequireAuthorization();
        apiGroupe.MapGroup("vehicule").AjouterRouteVehicule().RequireAuthorization();
        apiGroupe.MapGroup("fournisseur").AjouterRouteFournisseur().RequireAuthorization();
        apiGroupe.MapGroup("commande").AjouterRouteCommande().RequireAuthorization();
        apiGroupe.MapGroup("client").AjouterRouteClient().RequireAuthorization();
        apiGroupe.MapGroup("livraison").AjouterRouteLivraison().RequireAuthorization();
        apiGroupe.MapGroup("export").AjouterRouteExport().RequireAuthorization();
        apiGroupe.MapGroup("import").AjouterRouteImport().RequireAuthorization();

        return _app;
    }
}
