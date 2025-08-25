using Api.Extensions;
using Api.ModelsExports;
using Api.ModelsExports.Categories;
using Api.ModelsImports;
using Api.ModelsImports.Categories;
using Api.Services.Categories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Routes;

public static class CategorieRoute
{
    public static RouteGroupBuilder AjouterRouteCategorie(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les catégories du groupe")
            .Produces<CategorieExport[]>()
            .CacheOutput(NomCache.Categorie);

        builder.MapGet("listerPaginer", ListerPaginerAsync)
            .WithDescription("Lister les catégories du groupe paginé")
            .Produces<PaginationExport<CategorieExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter une nouvelle categorie au groupe ('IdPublic' pas pris en compte)")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicCategorie:guid}", ModifierAsync)
            .WithDescription("Modifier une categorie au groupe")
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] ICategorieService _categorieServ
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var liste = await _categorieServ.ListerAsync(idGroupe);

        return Results.Extensions.OK(liste, CategorieExportContext.Default);
    }

    async static Task<IResult> ListerPaginerAsync(
        HttpContext _httpContext,
        [FromServices] ICategorieService _categorieServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var liste = await _categorieServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<CategorieImport> _validator,
        [FromServices] ICategorieService _categorieServ,
        [FromServices] IOutputCacheStore _cache,
        [FromBody] CategorieImport _categorieImport
    )
    {
        var validate = _validator.Validate(_categorieImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        Guid idPublic = await _categorieServ.AjouterAsync(
            _categorieImport.Nom, 
            idGroupe
        );

        if(idPublic != Guid.Empty)
        {
            await _cache.EvictByTagAsync(NomCache.Categorie, default);
            return Results.Created("", idPublic);
        }

        return Results.BadRequest("Erreur d'ajout de la categorie");
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<CategorieImport> _validator,
        [FromServices] IOutputCacheStore _cache,
        [FromServices] ICategorieService _categorieServ,
        [FromRoute(Name = "idPublicCategorie")] Guid _idPublicCategorie,
        [FromBody] CategorieImport _categorieImport
    )
    {
        var validate = _validator.Validate(_categorieImport);

        if(!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        bool estModifier = await _categorieServ.ModifierAsync(
            _categorieImport.Nom,
            _idPublicCategorie,
            idGroupe
        );

        if (estModifier)
        {
            await _cache.EvictByTagAsync(NomCache.Categorie, default);
            return Results.NoContent();
        }

        return Results.NotFound("La categorie n'existe pas");
    }
}
