using Api.Extensions;
using Api.ModelsExports.Categories;
using Api.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class CategorieRoute
{
    public static RouteGroupBuilder AjouterRouteCategorie(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .Produces<CategorieExport[]>()
            .CacheOutput(NomCache.Categorie);

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] ICategorieService _categorieServ
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var liste = await _categorieServ.ListerAsync(idGroupe);

        return Results.Extensions.OK(liste, CategorieExportContext.Default);
    }
}
