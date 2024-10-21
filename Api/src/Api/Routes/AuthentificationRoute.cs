using Api.Extensions;

namespace Api.Routes;

public static class AuthentificationRoute
{
    public static RouteGroupBuilder AjouterRouteAuthentification(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        

        return builder;
    }
}
