using Api.Extensions;
using Api.Models;
using Api.ModelsExports.Commandes;
using Api.ModelsImports.Commandes;
using Api.Services.Clients;
using Api.Services.Commandes;
using Api.Services.Groupes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Mdp;

namespace Api.Routes;

public static class CommandeRoute
{
    public static RouteGroupBuilder AjouterRouteCommande(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les commandes (Status: 0 => Valider, 1 => En attente de validation, 2 => Annuler, 3 => Livrer (terminer), 4 => Tout")
            .Produces<CommandeExport[]>()
            .ProducesBadRequest();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter une nouvelle commande")
            .ProducesCreated<string>()
            .ProducesBadRequestErreurValidation();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] ICommandeService _commandeServ,
        [AsParameters] CommandeFiltreImport _commandeFiltre
    )
    {
        if (_commandeFiltre.DateDebut > _commandeFiltre.DateFin)
            return Results.BadRequest("Date de début ne peux pas être supérieur à la date de fin");

        int idGroupe = _httpContext.RecupererIdGroupe();

        var liste = await _commandeServ.ListerAsync(_commandeFiltre, idGroupe);

        return Results.Extensions.OK(liste, CommandeExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<CommandeImport> _validator,
        [FromServices] IClientService _clientServ,
        [FromServices] ICommandeService _commandeServ,
        [FromServices] IGroupeService _groupeServ,
        [FromServices] IMdpService _mdpServ,
        [FromBody] CommandeImport _commandeImport
    )
    {
        var validate = await _validator.ValidateAsync(_commandeImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        string idPublicClient = _commandeImport.IdPublicClient ?? _httpContext.RecupererIdPublique();

        int idGroupe = _httpContext.RecupererIdGroupe();
        string prefixGrp = await _groupeServ.PrefixAsync(idGroupe);
        int idClient = await _clientServ.RecupererIdAsync(idPublicClient);
        string numero = _mdpServ.Generer(12, false);

        Commande commande = new()
        {
            IdClient = idClient,
            Numero = $"{prefixGrp}{numero}",
            DatePourLe = _commandeImport.Date.ToDateTime(TimeOnly.MinValue)
        };

        bool retour = await _commandeServ.AjouterAsync(commande, _commandeImport.ListeProduit);

        return retour ? Results.Created("", commande.Numero) : Results.BadRequest("Erreur d'ajout");
    }
}
