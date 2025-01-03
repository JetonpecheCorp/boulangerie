using Api.Extensions;
using Api.ModelsExports;
using Api.ModelsExports.Livraisons;
using Api.ModelsImports.Livraisons;
using Api.Services.Livraisons;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class LivraisonRoute
{
    public static RouteGroupBuilder AjouterRouteLivraison(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les livraisons")
            .Produces<PaginationExport<LivraisonExport>>();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] ILivraisonService _livraisonServ,
        [AsParameters] LivraisonFiltreImport _filtre
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var retour = await _livraisonServ.ListerAsync(_filtre, idGroupe);

        return Results.Extensions.OK(retour, PaginationExportContext.Default);
    }
}
