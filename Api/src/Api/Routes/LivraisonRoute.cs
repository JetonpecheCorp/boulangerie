using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Livraisons;
using Api.ModelsImports.Livraisons;
using Api.Services.Groupes;
using Api.Services.Livraisons;
using Api.Services.Utilisateurs;
using Api.Services.Vehicules;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Mdp;

namespace Api.Routes;

public static class LivraisonRoute
{
    public static RouteGroupBuilder AjouterRouteLivraison(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les livraisons, Recherche prioritaire non cumulable (numero de livraison)")
            .Produces<PaginationExport<LivraisonExport>>();

        builder.MapGet("detail/{idPublicLivraison:guid}", DetailAsync)
            .WithDescription("Detail de la livraison")
            .ProducesNotFound()
            .Produces<LivraisonDetailExport>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter une livraison")
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicLivraison:guid}", ModifierAsync)
            .WithDescription("Modifier une livraison")
            .ProducesBadRequestErreurValidation()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] ILivraisonService _livraisonServ,
        [AsParameters] LivraisonFiltreImport _filtre
    )
    {
        if (_filtre.NbParPage <= 0)
            _filtre.NbParPage = 20;

        if(_filtre.NumPage <= 0)
            _filtre.NumPage = 1;

        int idGroupe = _httpContext.RecupererIdGroupe();

        var retour = await _livraisonServ.ListerAsync(_filtre, idGroupe);

        return Results.Extensions.OK(retour, PaginationExportContext.Default);
    }

    async static Task<IResult> DetailAsync(
        HttpContext _httpContext,
        [FromServices] ILivraisonService _livraisonServ,
        [FromRoute(Name = "idPublicLivraison")] Guid _idPublicLivraison
    )
    {
        var info = await _livraisonServ.RecupererDetailAsync(_idPublicLivraison);

        return info is not null ? Results.Extensions.OK(info, LivraisonDetailExportContext.Default) : Results.NotFound("La livraison n'existe pas");
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<LivraisonImport> _validator,
        [FromServices] ILivraisonService _livraisonServ,
        [FromServices] IUtilisateurService _utilisateurServ,
        [FromServices] IVehiculeService _vehiculeServ,
        [FromServices] IMdpService _mdpServ,
        [FromServices] IGroupeService _groupeServ,
        [FromBody] LivraisonImport _livraison
    )
    {
        var validate = await _validator.ValidateAsync(_livraison);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();
        string prefix = await _groupeServ.PrefixAsync(idGroupe);

        int idUtilisateur = await _utilisateurServ.RecupererId(_livraison.IdPublicConducteur, idGroupe);
        int idVehicule = await _vehiculeServ.RecupererId(_livraison.IdPublicVehicule, idGroupe);

        Livraison livraison = new()
        {
            Date = _livraison.Date,
            IdPublic = Guid.NewGuid(),
            IdUtilisateur = idUtilisateur,
            IdVehicule = idVehicule,
            Frais = _livraison.Frais,
            Numero = prefix + _mdpServ.Generer(17, false)
        };

        int id = await _livraisonServ.AjouterAsync(livraison, _livraison.Liste);

        return Results.Created("", new { idPublic = livraison.IdPublic, numero = livraison.Numero });
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<LivraisonImport> _validator,
        [FromServices] ILivraisonService _livraisonServ,
        [FromBody] LivraisonImport _livraison,
        [FromRoute(Name = "idPublicLivraison")] Guid _idPublicLivraison
    )
    {
        var validate = await _validator.ValidateAsync(_livraison);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        await _livraisonServ.ModifierAsync(_idPublicLivraison, idGroupe, _livraison);

        return Results.NoContent();
    }
}
