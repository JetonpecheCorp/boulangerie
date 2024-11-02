using Api.Extensions;

namespace Api.Routes;

public static class RecetteRoute
{
    public static RouteGroupBuilder AjouterRouteRecette(this  RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapPost("ajouter", AjouterAsync);

        return builder;
    }

    async static Task<IResult> AjouterAsync(HttpContext _httpContext)
    {


        return Results.Ok();
    }
}
