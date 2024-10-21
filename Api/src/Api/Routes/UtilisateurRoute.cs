using Api.Extensions;
using Api.Models;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Mdp;

namespace Api.Routes;

public static class UtilisateurRoute
{
    public static RouteGroupBuilder AjouterRouteUtilisateur(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouvelle utilisateur")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated();

        return builder;
    }

    async static Task<IResult> AjouterAsync(
        [FromServices] IValidator<UtilisateurImport> validator,
        [FromServices] IMdpService _mdpServ,
        [FromServices] IUtilisateurService _utilisateurServ,
        [FromBody] UtilisateurImport _utilisateurImport
    )
    {
        try
        {
            var validate = await validator.ValidateAsync(_utilisateurImport);

            if (!validate.IsValid)
                return Results.Extensions.ErreurValidator(validate.Errors);

            string mdpHasher = _mdpServ.Hasher(_utilisateurImport.Mdp);

            Utilisateur utilisateur = new()
            {
                IdGroupe = _utilisateurImport.IdGroupe,
                Nom = _utilisateurImport.Nom.XSS(),
                Prenom = _utilisateurImport.Prenom.XSS(),
                Telephone = _utilisateurImport.Telephone?.XSS(),
                IdPublic = Guid.NewGuid(),
                Mail = _utilisateurImport.Mail,
                Mdp = mdpHasher
            };

            await _utilisateurServ.AjouterAsync(utilisateur);

            return Results.Created("", utilisateur.Id);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }
}
