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

        builder.MapGet("listerLeger", ListerLegerAsync)
            .WithDescription("Lister des produits de façon allégé")
            .Produces<PaginationExport<ProduitLegerExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau produit")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        builder.MapPut("modifier", ModifierAsync)
           .WithDescription("Modifier un produit")
           .ProducesNotFound()
           .ProducesNoContent();

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

        int idGroupe = _httpContext.RecupererIdGroupe();

        var paginationExport = await _produitServ.ListerAsync(
            _pagination,
            idGroupe
        );

        return Results.Extensions.OK(paginationExport, PaginationExportContext.Default);
    }

    async static Task<IResult> ListerLegerAsync(
    HttpContext _httpContext,
    [AsParameters] PaginationImport _pagination,
    [FromServices] IProduitService _produitServ
    )
    {
        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        int idGroupe = _httpContext.RecupererIdGroupe();

        var paginationExport = await _produitServ.ListerLegerAsync(
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

        return Results.Created("", produit.IdPublic);
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<ProduitImport> _validator,
        [FromServices] IProduitService _produitServ,
        [FromServices] ICategorieService _categorieServ,
        [FromBody] ProduitImport _produitImport
    )
    {
        _produitImport.Mode = Enums.EModeImport.Modifier;

        var validate = await _validator.ValidateAsync(_produitImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();
        int idCategorie = await _categorieServ.RecupererIdAsync(_produitImport.IdPublicCategorie, idGroupe);

        SetPropertyBuilder<Produit> builder = new();

        builder.SetProperty(x => x.Alergene, string.Join(',', _produitImport.ListeAllergene))
            .SetProperty(x => x.IdCategorie, idCategorie)
            .SetProperty(x => x.IdTva, _produitImport.IdTva)
            .SetProperty(x => x.Nom, _produitImport.Nom.XSS())
            .SetProperty(x => x.PrixHt, _produitImport.PrixHt)
            .SetProperty(x => x.CodeInterne, _produitImport.CodeInterne)
            .SetProperty(x => x.Poids, _produitImport.Poids)
            .SetProperty(x => x.Stock, _produitImport.Stock)
            .SetProperty(x => x.StockAlert, _produitImport.StockAlert);

        bool estModifier = await _produitServ.ModifierAsync(idGroupe, _produitImport.IdPublic!, builder);

        return estModifier ? Results.NoContent() : Results.NotFound("Le produit n'existe pas");
    }
}
