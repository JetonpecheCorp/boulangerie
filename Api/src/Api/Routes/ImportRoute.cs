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

        builder.MapPost("utilisateur", UtilisateurAsync)
            .DisableAntiforgery();

        return builder;
    }

    async static Task<IResult> UtilisateurAsync(
        HttpContext _httpContext,
        [FromServices] IImportService _importServ,
        [FromForm(Name = "fichier")] IFormFile _fichierCSV
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var retour = await _importServ.UtilisateurAsync(idGroupe, _fichierCSV);

        return Results.Extensions.OK(retour, ErreurValidationCSVContext.Default);
    }
}
