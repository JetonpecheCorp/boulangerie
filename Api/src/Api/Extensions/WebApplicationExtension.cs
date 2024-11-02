using Api.Routes;

namespace Api.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication AjouterRouteAPI(this WebApplication _app)
    {
        _app.MapGroup("utilisateur").AjouterRouteUtilisateur();
        _app.MapGroup("groupe").AjouterRouteGroupe();
        _app.MapGroup("authentification").AjouterRouteAuthentification();
        _app.MapGroup("ingredient").AjouterRouteIngredient();
        _app.MapGroup("recette").AjouterRouteRecette();
        _app.MapGroup("produit").AjouterRouteProduit();
        _app.MapGroup("tva").AjouterRouteTva();
        _app.MapGroup("categorie").AjouterRouteCategorie();

        return _app;
    }
}
