using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Vehicules;
using Api.ModelsImports;
using Api.ModelsImports.Vehicules;
using Api.Services.Vehicules;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class VehiculeRoute
{
    public static RouteGroupBuilder AjouterRouteVehicule(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister des véhicules")
            .Produces<PaginationExport<VehiculeExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau véhicule")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicVehicule}", ModifierAsync)
            .WithDescription("Modifier un véhicule")
            .ProducesBadRequestErreurValidation()
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] IVehiculeService _vehiculeServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        int idGroupe = _httpContext.RecupererIdGroupe();

        var infos = await _vehiculeServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(infos, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<VehiculeImport> _validator,
        [FromServices] IVehiculeService _vehiculeServ,
        [FromBody] VehiculeImport _vehiculeImport
    )
    {
        var validate = _validator.Validate(_vehiculeImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        Vehicule vehicule = new()
        {
            IdGroupe = idGroupe,
            IdPublic = Guid.NewGuid(),
            Immatriculation = _vehiculeImport.Immatriculation.XSS(),
            InfoComplementaire = _vehiculeImport.InfoComplementaire?.XSS()
        };

        await _vehiculeServ.AjouterAsync(vehicule);

        return Results.Created("", vehicule.IdPublic);
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<VehiculeImport> _validator,
        [FromServices] IVehiculeService _vehiculeServ,
        [FromRoute(Name = "idPublicVehicule")] string _idPublicVehicule,
        [FromBody] VehiculeImport _vehiculeImport
    )
    {
        var validate = _validator.Validate(_vehiculeImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        bool estModifier = await _vehiculeServ.ModifierAsync(
            _vehiculeImport.Immatriculation.XSS(),
            _vehiculeImport.InfoComplementaire?.XSS(),
            _idPublicVehicule, idGroupe
        );

        return estModifier ? Results.NoContent() : Results.NotFound("Le véhicule n'existe pas");
    }
}
