using Api.Extensions;
using Api.Models;
using Api.ModelsExports.Groupes;
using Api.ModelsImports.Groupes;
using Api.Services.Groupes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Mdp;

namespace Api.Routes;

public static class GroupeRoute
{
    public static RouteGroupBuilder AjouterRouteGroupe(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("Lister", ListerAsync)
            .WithDescription("Ajouter un nouveau groupe")
            .Produces<GroupeExport[]>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau groupe")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<int>();

        builder.MapPut("bloquer-debloquer/{idGroupe:int}", BloquerDebloquerAsync)
            .WithDescription("Bloquer ou debloquer la connexion à un groupe")
            .ProducesNotFound()
            .ProducesNoContent();

        builder.MapPut("modifier/{idGroupe:int}", ModifierAsync)
            .WithDescription("Modifier un groupe")
            .ProducesBadRequestErreurValidation()
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync([FromServices] IGroupeService _groupeServ)
    {
        var tab = await _groupeServ.ListerAsync();

        return Results.Extensions.OK(tab, GroupeExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        [FromServices] IValidator<GroupeImport> _validator,
        [FromServices] IGroupeService _groupeServ,
        [FromServices] IMdpService _mdpServ,
        [FromBody] GroupeImport _groupeImport
    )
    {
        var validate = _validator.Validate(_groupeImport);

        if(!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        Groupe groupe = new()
        {
            Adresse = _groupeImport.Adresse,
            Nom = _groupeImport.Nom,
            Prefix = _groupeImport.Nom.Substring(0, 3)
        };

        await _groupeServ.AjouterAsync(groupe);

        return Results.Created("", groupe.Id);
    }

    async static Task<IResult> BloquerDebloquerAsync(
        [FromRoute(Name = "idGroupe")] int _idGroupe,
        [FromServices] IGroupeService _groupeServ
    )
    {
        if (_idGroupe <= 0)
            return Results.NotFound();

        SetPropertyBuilder<Groupe> builder = new();
        builder.SetProperty(x => x.ConnexionBloquer, y => !y.ConnexionBloquer);

        bool ok = await _groupeServ.ModifierAsync(_idGroupe, builder);

        return ok ? Results.NoContent() : Results.NotFound();
    }

    async static Task<IResult> ModifierAsync(
        [FromServices] IValidator<GroupeImport> _validator,
        [FromServices] IGroupeService _groupeServ,
        [FromRoute(Name = "idGroupe")] int _idGroupe,
        [FromBody] GroupeImport _groupeImport
    )
    {
        if (_idGroupe <= 0)
            return Results.NotFound();

        var validate = _validator.Validate(_groupeImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        SetPropertyBuilder<Groupe> builder = new();
        builder.SetProperty(x => x.Nom, _groupeImport.Nom.XSS())
            .SetProperty(x => x.Adresse, _groupeImport.Adresse.XSS());

        bool ok = await _groupeServ.ModifierAsync(_idGroupe, builder);

        return ok ? Results.NoContent() : Results.NotFound();
    }
}
