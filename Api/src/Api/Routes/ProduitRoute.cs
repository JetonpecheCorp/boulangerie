using Api.Extensions;
using Api.Models;
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

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un nouveau produit")
            .ProducesBadRequestErreurValidation()
            .ProducesCreated<string>();

        return builder;
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
            DateCreation = DateOnly.FromDateTime(DateTime.UtcNow),
            DateModification = DateOnly.FromDateTime(DateTime.UtcNow),
            Stock = _produitImport.Stock,
            StockAlert = _produitImport.StockAlert
        };

        await _produitServ.AjouterAsync(produit);

        return Results.Created();
    }
}
