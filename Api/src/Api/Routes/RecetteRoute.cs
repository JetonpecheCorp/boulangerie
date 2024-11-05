using Api.Extensions;
using Api.ModelsExports.Recettes;
using Api.ModelsImports.Recettes;
using Api.Services.Recettes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class RecetteRoute
{
    public static RouteGroupBuilder AjouterRouteRecette(this  RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister/{idPublicProduit}", ListerAsync)
            .WithDescription("Lister les ingredients de la recette")
            .Produces<RecetteExport[]>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un ");

        builder.MapDelete("supprimer", SupprimerAsync)
            .WithDescription("Supprimer un ingredient d'une recette d'un produit")
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromRoute(Name = "idPublicProduit")] string _idPublicProduit,
        IRecetteService _recetteServ
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var liste = await _recetteServ.ListerAsync(_idPublicProduit, idGroupe);

        return Results.Extensions.OK(liste, RecetteExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext
    )
    {
        return Results.NoContent();
    }

    async static Task<IResult> SupprimerAsync(
        HttpContext _httpContext,
        [FromServices] IRecetteService _recetteServ,
        [FromBody] RecetteSupprimerImport _recetteImport
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        bool estSupprimer = await _recetteServ.SupprimerAsync(
            _recetteImport.IdPublicProduit,
            _recetteImport.IdPublicIngredient,
            idGroupe
        );

        return estSupprimer ? Results.NoContent() : Results.NotFound("L'ingredient dans la recette n'existe pas");
    }
}
