using Api.Enums;
using Api.Extensions;
using Api.ModelsExports;
using Api.Services.Imports;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class ImportRoute
{
    public static RouteGroupBuilder AjouterRouteImport(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapPost("{ressource}", ImportAsync)
            .WithDescription("""
                Importer des infos avec un CSV 
                Valeurs possible de ressource => utilisateur, client, ingredient, fournisseur (non sensible à la case). 
                Si aucune erreur => import OK, sinon l'import est fait SAUF sur les lignes où il y a des erreurs.
            """)
            
            .DisableAntiforgery();

        return builder;
    }

    async static Task<IResult> ImportAsync(
        HttpContext _httpContext,
        [FromServices] IImportService _importServ,
        [FromRoute(Name = "ressource")] string _ressource,
        [FromForm(Name = "fichier")] IFormFile _fichierCSV
    )
    {
        if(Enum.TryParse<ERessourceImport>(_ressource, true, out var ressource))
        {
            int idGroupe = _httpContext.RecupererIdGroupe();

            var retour = ressource switch
            {
                ERessourceImport.Utilisateur => await _importServ.UtilisateurAsync(idGroupe, _fichierCSV),
                ERessourceImport.Client => await _importServ.ClientAsync(idGroupe, _fichierCSV),
                ERessourceImport.Ingredient => await _importServ.IngredientAsync(idGroupe, _fichierCSV),
                ERessourceImport.Fournisseur => await _importServ.FournisseurAsync(idGroupe, _fichierCSV),
                _ => []
            };

            return Results.Extensions.OK(retour, ErreurValidationCSVContext.Default);
        }

        return Results.BadRequest("La ressource n'existe pas");
    }
}
