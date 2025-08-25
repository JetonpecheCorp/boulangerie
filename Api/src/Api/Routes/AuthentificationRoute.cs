using Api.Extensions;
using Api.Models;
using Api.ModelsExports.Connexions;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Jwts;
using Services.Mail;
using Services.Mdp;
using System.Security.Claims;

namespace Api.Routes;

public static class AuthentificationRoute
{
    public static RouteGroupBuilder AjouterRouteAuthentification(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable().RequireRateLimiting("connexion-limiteur");

        builder.MapPost("connexion", ConnexionAsync)
            .ProducesBadRequest()
            .ProducesToManyRequests()
            .Produces<ConnexionExport>();

        builder.MapGet("demande-reset-mdp/{mail}", DemandeResetMdpAsync)
            .ProducesToManyRequests()
            .ProducesBadRequest()
            .ProducesNoContent();

        builder.MapPost("reset-mdp", ResetMdpAsync)
            .RequireAuthorization(NomPolicyJwt.ResetMdp)
            .ProducesNotFound()
            .ProducesToManyRequests()
            .ProducesBadRequestErreurValidation()
            .ProducesNoContent();

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
            new Claim(ClaimTypes.Role, utilisateur.EstAdmin ? "admin" : "client"),
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

    async static Task<IResult> DemandeResetMdpAsync(
        [FromRoute(Name = "mail")] string _mail,
        [FromServices] IUtilisateurService _utilisateurServ,
        [FromServices] IJwtService _jwtServ,
        [FromServices] IMailService _mailServ
    )
    {
        if (_mail.MailInvalide())
            return Results.BadRequest("Ce n'est pas une adresse mail");

        if (!await _utilisateurServ.MailExisteAsync(_mail))
            return Results.NotFound();

        var utilisateur = await _utilisateurServ.InfoAsync(_mail);

        string jwt = _jwtServ.Generer([
            new Claim(ClaimTypes.Role, "mdp-oublie"),
            new Claim("idGroupe", utilisateur!.IdGroupe.ToString()),
            new Claim("idUtilisateur", utilisateur!.IdPublic.ToString())
        ]);

        string message = $"""
            Bonjour {utilisateur.Nom} {utilisateur.Prenom}, <br><br>

            Vous avez demandé de changer votre mot de passe sur l'application Boulangerie. <br>
            Veuillez clicker <a href="http://localhost:4200/#/reset-mdp?p={jwt}">ici</a>. <br>
            ATTENTION: Ce lien est valide 5 min.<br>
            <b>Si vous n'êtes pas l'auteur de cette demande, ignorer simplement ce mail </b>
            <br><br>

            Cordialement, l'équipe Boulangerie
            """;

        await _mailServ.EnvoyerAsync([utilisateur.Mail], "Changement du mot de passe", message, true);

        return Results.NoContent();
    }

    async static Task<IResult> ResetMdpAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<ResetMdpImport> _validator,
        [FromServices] IUtilisateurService _utilisateurServ,
        [FromServices] IMdpService _mdpServ,
        [FromBody] ResetMdpImport _info
    )
    {
        var validate = _validator.Validate(_info);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        string idUtilisateur = _httpContext.RecupererIdPublique();
        int idGroupe = _httpContext.RecupererIdGroupe();

        SetPropertyBuilder<Utilisateur> builder = new();

        builder.SetProperty(x => x.Mdp, _mdpServ.Hasher(_info.Mdp));

        bool ok = await _utilisateurServ.ModifierAsync(builder, idGroupe, Guid.Parse(idUtilisateur));

        return ok ? Results.NoContent() : Results.NotFound();
    }
}
