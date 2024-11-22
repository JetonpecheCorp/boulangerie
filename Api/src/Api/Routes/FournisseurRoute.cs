using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Fournisseurs;
using Api.ModelsImports;
using Api.ModelsImports.Fournisseurs;
using Api.Services.Fournisseurs;
using Api.Services.Ingredients;
using Api.Services.Produits;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class FournisseurRoute
{
    public static RouteGroupBuilder AjouterRouteFournisseur(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les fournisseurs paginer d'un groupe (si <= 0: page: 1, nb par page: 20)")
            .Produces<PaginationExport<FournisseurExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un fournisseur")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicFournisseur}", ModifierAsync)
            .WithDescription("Modifier un fournisseur")
            .ProducesBadRequestErreurValidation()
            .ProducesNotFound()
            .ProducesNoContent();

        builder.MapDelete("archiver/{idPublicFournisseur}", ArchiverAsync)
            .WithDescription("Archiver un fournisseur")
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] IFournisseurService _fournisseurServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var paginationExport = await _fournisseurServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(paginationExport, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<FournisseurImport> _validator,
        [FromServices] IFournisseurService _fournisseurServ,
        [FromBody] FournisseurImport _fournisseurImport
    )
    {
        var validate = await _validator.ValidateAsync(_fournisseurImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        Fournisseur fournisseur = new()
        {
             IdGroupe = idGroupe,
             IdPublic = Guid.NewGuid(),
             Adresse = _fournisseurImport.Adresse,
             Telephone = _fournisseurImport.Telephone,
             Nom = _fournisseurImport.Nom,
             Mail = _fournisseurImport.Mail
        };

        await _fournisseurServ.AjouterAsync(
            fournisseur, 
            _fournisseurImport.ListeIdPublicIngredient, 
            _fournisseurImport.ListeIdPublicProduit
        );

        return Results.Created("", fournisseur.IdPublic);
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<FournisseurImport> _validator,
        [FromServices] IFournisseurService _fournisseurServ,
        [FromServices] IProduitService _produitServ,
        [FromServices] IIngredientService _ingredientServ,
        [FromRoute(Name = "idPublicFournisseur")] string _idPublicFournisseur,
        [FromBody] FournisseurImport _fournisseurImport
    )
    {
        var validate = await _validator.ValidateAsync(_fournisseurImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        SetPropertyBuilder<Fournisseur> builder = new();

        builder.SetProperty(x => x.Adresse, _fournisseurImport.Adresse)
            .SetProperty(x => x.Nom, _fournisseurImport.Nom)
            .SetProperty(x => x.Telephone, _fournisseurImport.Telephone)
            .SetProperty(x => x.Mail, _fournisseurImport.Mail)
            .SetProperty(x => x.DateModification, DateOnly.FromDateTime(DateTime.UtcNow));

        bool ok = await _fournisseurServ.ModifierAsync(idGroupe, _idPublicFournisseur, builder, _fournisseurImport.ListeIdPublicProduit, _fournisseurImport.ListeIdPublicIngredient);

        return ok ? Results.NoContent() : Results.NotFound();
    }

    async static Task<IResult> ArchiverAsync(
        HttpContext _httpContext,
        [FromServices] IFournisseurService _fournisseurServ,
        [FromRoute(Name = "idPublicFournisseur")] string _idPublicFournisseur
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        bool ok = await _fournisseurServ.ArchiverAsync(idGroupe, _idPublicFournisseur);

        return ok ? Results.NoContent() : Results.NotFound();
    }
}
