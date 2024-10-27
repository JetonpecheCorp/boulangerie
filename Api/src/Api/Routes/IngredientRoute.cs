using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Ingredients;
using Api.ModelsImports;
using Api.ModelsImports.Ingredients;
using Api.Services.Ingredients;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class IngredientRoute
{
    public static RouteGroupBuilder AjouterRouteIngredient(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les ingredients paginer d'un groupe (si <= 0: page: 1, nb par page: 20)")
            .Produces<PaginationExport<IngredientExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau ingredient")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [AsParameters] PaginationImport _pagination,
        [FromServices] IIngredientService _ingredientServ
    )
    {
        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if(_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        int idGroupe = _httpContext.RecupererIdGroupe();

        var paginationExport = await _ingredientServ.ListerAsync(
            _pagination.NumPage,
            _pagination.NbParPage,
            idGroupe
        );

        return Results.Extensions.OK(paginationExport, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<IngredientImport> _validator,
        [FromServices] IIngredientService _ingredientServ,
        [FromBody] IngredientImport _ingredientImport
    )
    {
        var validate = _validator.Validate(_ingredientImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        Ingredient ingredient = new()
        {
            IdGroupe = idGroupe,
            IdPublic = Guid.NewGuid(),
            CodeInterne = _ingredientImport.CodeInterne,
            Nom = _ingredientImport.Nom,
            Stock = _ingredientImport.Stock,
            StockAlert = _ingredientImport.StockAlert
        };

        await _ingredientServ.AjouterAsync(ingredient);

        return Results.Created("", ingredient.IdPublic);
    }
}
