using Api.Extensions;
using Api.ModelsExports.Connexions;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Jwts;
using Services.Mdp;
using System.Security.Claims;

namespace Api.Routes;

public static class AuthentificationRoute
{
    public static RouteGroupBuilder AjouterRouteAuthentification(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapPost("connexion", ConnexionAsync);

        return builder;
    }

    async static Task<IResult> ConnexionAsync(
        [FromServices] IValidator<ConnexionImport> _validator,
        [FromServices] IJwtService _jwtServ,
        [FromServices] IMdpService _mdpServ,
        [FromServices] IUtilisateurService _utilisateurService,
        [FromBody] ConnexionImport _connexionImport
    )
    {
        var validate = _validator.Validate(_connexionImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);
            
        var utilisateur = await _utilisateurService.InfoAsync(_connexionImport.Login);

        if(utilisateur is null)
            return Results.BadRequest("Login ou mot de passe incorrect");

        if(!_mdpServ.VerifierHash(_connexionImport.Mdp, utilisateur.Mdp))
            return Results.BadRequest("Login ou mot de passe incorrect");

        string jwt = _jwtServ.Generer([
            new Claim("idUtilisateur", utilisateur.IdPublic.ToString()),
            new Claim("idGroupe", utilisateur.IdGroupe.ToString())
        ]);

        var export = new ConnexionExport
        {
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Jwt = jwt
        };

        return Results.Extensions.OK(export, ConnexionExportContext.Default);
    }
}
