using Api.Extensions;
using Api.ModelsExports.Tvas;
using Api.Services.Tvas;

namespace Api.Routes;

public static class TvaRoute
{
    public static RouteGroupBuilder AjouterRouteTva(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .Produces<TvaExport[]>()
            .CacheOutput(NomCache.Tva);

        return builder;
    }

    async static Task<IResult> ListerAsync(ITvaService _tvaServ)
    {
        var liste = await _tvaServ.ListerAsync();

        return Results.Extensions.OK(liste, TvaExportContext.Default);
    }
}
