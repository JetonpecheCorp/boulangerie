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
            .WithDescription("Ajouter un nouveau ingredient ('IdPublic' pas pris en compte ici)")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier", ModifierAsync)
            .WithDescription("Modifier un ingredient")
            .ProducesBadRequestErreurValidation()
            .ProducesNoContent()
            .ProducesNotFound();

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
            _pagination,
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

    async static Task<IResult> ModifierAsync(
       HttpContext _httpContext,
       [FromServices] IValidator<IngredientImport> _validator,
       [FromServices] IIngredientService _ingredientServ,
       [FromBody] IngredientImport _ingredientImport
    )
    {
        _ingredientImport.Mode = Enums.EModeImport.Modifier;

        var validate = _validator.Validate(_ingredientImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        SetPropertyBuilder<Ingredient> builder = new();

        builder.SetProperty(x => x.Stock, _ingredientImport.Stock)
            .SetProperty(x => x.StockAlert, _ingredientImport.StockAlert)
            .SetProperty(x => x.Nom, _ingredientImport.Nom)
            .SetProperty(x => x.CodeInterne, _ingredientImport.CodeInterne);

        bool estModifier = await _ingredientServ.ModifierAsync(idGroupe, _ingredientImport.IdPublic!.Value, builder);

        return estModifier ? Results.NoContent() : Results.NotFound("La ressource n'existe pas");
    }
}
