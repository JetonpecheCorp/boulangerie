using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Produits;
using Api.ModelsImports;
using Api.ModelsImports.Produits;
using Api.Services.Categories;
using Api.Services.Produits;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class ProduitRoute
{
    public static RouteGroupBuilder AjouterRouteProduit(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister des produits")
            .Produces<PaginationExport<ProduitExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau produit")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [AsParameters] PaginationImport _pagination,
        [FromServices] IProduitService _produitServ
    )
    {
        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        int idGroupe = 1;// _httpContext.RecupererIdGroupe();

        var paginationExport = await _produitServ.ListerAsync(
            _pagination,
            idGroupe
        );

        return Results.Extensions.OK(paginationExport, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<ProduitImport> _validator,
        [FromServices] IProduitService _produitServ,
        [FromServices] ICategorieService _categorieServ,
        [FromBody] ProduitImport _produitImport
    )
    {
        var validate = await _validator.ValidateAsync(_produitImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();
        int idCategorie = await _categorieServ.RecupererIdAsync(_produitImport.IdPublicCategorie, idGroupe);

        Produit produit = new()
        {
            IdGroupe = idGroupe,
            IdCategorie = idCategorie,
            IdTva = _produitImport.IdTva,
            IdPublic = Guid.NewGuid(),
            Nom = _produitImport.Nom.XSS(),
            PrixHt = _produitImport.PrixHt,
            Alergene = string.Join(",", _produitImport.ListeAllergene),
            CodeInterne = _produitImport.CodeInterne,
            Poids = _produitImport.Poids,
            Stock = _produitImport.Stock,
            StockAlert = _produitImport.StockAlert
        };

        await _produitServ.AjouterAsync(produit);

        return Results.Created();
    }
}
