using Api.Extensions;
using Api.ModelsExports;
using Api.ModelsExports.Clients;
using Api.ModelsImports;
using Api.Services.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class ClientRoute
{
    public static RouteGroupBuilder AjouterRouteClient(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les clients")
            .Produces<PaginationExport<ClientExport>>();

        builder.MapGet("listerLeger", ListerLegerAsync)
            .WithDescription("Lister les clients de façon allégé")
            .Produces<PaginationExport<ClientLegerExport>>();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] IClientService _clientServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var liste = await _clientServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
    }

    async static Task<IResult> ListerLegerAsync(
        HttpContext _httpContext,
        [FromServices] IClientService _clientServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var liste = await _clientServ.ListerLegerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
    }
}
