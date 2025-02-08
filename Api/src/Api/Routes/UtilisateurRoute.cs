using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsImports;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Mdp;
using Api.ModelsExports.Utilisateurs;

namespace Api.Routes;

public static class UtilisateurRoute
{
    public static RouteGroupBuilder AjouterRouteUtilisateur(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
           .WithDescription("Lister les utilisateurs")
           .ProducesBadRequestErreurValidation()
           .Produces<PaginationExport<UtilisateurExport>>();

        builder.MapGet("listerLeger", ListerLegerAsync)
            .WithDescription("Lister les utilisateurs de façon allégé")
            .ProducesBadRequestErreurValidation()
            .Produces<PaginationExport<UtilisateurLegerExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouvelle utilisateur")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicUtilisateur:guid}", ModifierAsync)
            .WithDescription("Modifier un utilisateur")
            .ProducesBadRequestErreurValidation()
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] IUtilisateurService _utilisateurServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if(_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if(_pagination.NbParPage <= 0)
            _pagination.NbParPage = 10;

        var retour = await _utilisateurServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(retour, PaginationExportContext.Default);
    }

    async static Task<IResult> ListerLegerAsync(
        HttpContext _httpContext,
        [FromServices] IUtilisateurService _utilisateurServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 10;

        var retour = await _utilisateurServ.ListerLegerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(retour, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
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
                IdGroupe = _httpContext.RecupererIdGroupe(),
                Nom = _utilisateurImport.Nom.XSS(),
                Prenom = _utilisateurImport.Prenom.XSS(),
                Telephone = _utilisateurImport.Telephone?.XSS(),
                IdPublic = Guid.NewGuid(),
                Mail = _utilisateurImport.Mail,
                Mdp = mdpHasher
            };

            await _utilisateurServ.AjouterAsync(utilisateur);

            return Results.Created("", utilisateur.IdPublic);
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<UtilisateurModifierImport> validator,
        [FromServices] IUtilisateurService _utilisateurServ,
        [FromRoute(Name = "idPublicUtilisateur")] Guid _idPublicUtilisateur,
        [FromBody] UtilisateurModifierImport _utilisateurImport
    )
    {
        try
        {
            _utilisateurImport.IdPublic = _idPublicUtilisateur;

            var validate = await validator.ValidateAsync(_utilisateurImport);

            if (!validate.IsValid)
                return Results.Extensions.ErreurValidator(validate.Errors);

            var builder = new SetPropertyBuilder<Utilisateur>();

            builder.SetProperty(x => x.Nom, _utilisateurImport.Nom.XSS())
                .SetProperty(x => x.Prenom, _utilisateurImport.Prenom.XSS())
                .SetProperty(x => x.Mail, _utilisateurImport.Mail.XSS());

            if (_utilisateurImport.Telephone is not null)
                builder.SetProperty(x => x.Telephone, _utilisateurImport.Telephone.XSS());

            var ok = await _utilisateurServ.ModifierAsync(
                builder,
                _httpContext.RecupererIdGroupe(),
                _utilisateurImport.IdPublic
            );

            return ok ? Results.NoContent() : Results.NotFound();
        }
        catch
        {
            return Results.Extensions.ErreurConnexionBdd();
        }
    }
}
