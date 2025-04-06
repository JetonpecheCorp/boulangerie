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
}
