using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Clients;
using Api.ModelsImports;
using Api.Services.Clients;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class ClientRoute
{
    public static RouteGroupBuilder AjouterRouteClient(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("Lister les clients")
            .Produces<PaginationExport<ClientExport>>();

        builder.MapGet("listerLeger", ListerLegerAsync)
            .WithDescription("Lister les clients de façon allégé")
            .Produces<PaginationExport<ClientLegerExport>>();

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter un client")
            .ProducesBadRequestErreurValidation()
            .ProducesBadRequest()
            .ProducesCreated<string>();

        builder.MapPut("modifier/{idPublicClient:guid}", ModifierAsync)
            .WithDescription("Modifier un client")
            .ProducesBadRequestErreurValidation()
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    async static Task<IResult> ListerAsync(
        HttpContext _httpContext,
        [FromServices] IClientService _clientServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var liste = await _clientServ.ListerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
    }

    async static Task<IResult> ListerLegerAsync(
        HttpContext _httpContext,
        [FromServices] IClientService _clientServ,
        [AsParameters] PaginationImport _pagination
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        if (_pagination.NumPage <= 0)
            _pagination.NumPage = 1;

        if (_pagination.NbParPage <= 0)
            _pagination.NbParPage = 20;

        var liste = await _clientServ.ListerLegerAsync(_pagination, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
    }

    async static Task<IResult> AjouterAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<ClientImport> _validator,
        [FromServices] IClientService _clientServ,
        [FromBody] ClientImport _clientImport
    )
    {
        var validate = await _validator.ValidateAsync(_clientImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        Client client = new()
        {
            IdGroupe = idGroupe,
            Mail = string.IsNullOrWhiteSpace(_clientImport.Mail) ? null : _clientImport.Mail,
            Telephone = string.IsNullOrWhiteSpace(_clientImport.Telephone) ? null : _clientImport.Telephone.XSS(),
            IdPublic = Guid.NewGuid(),
            InfoComplementaire = string.IsNullOrWhiteSpace(_clientImport.InfoComplementaire) ? null : _clientImport.InfoComplementaire.XSS(),
            Nom = _clientImport.Nom.XSS(),
            Adresse = _clientImport.Adresse.XSS(),
            AdresseFacturation = string.IsNullOrWhiteSpace(_clientImport.AdresseFacturation) ? _clientImport.Adresse.XSS() : _clientImport.AdresseFacturation.XSS(),
        };

        await _clientServ.AjouterAsync(client);

        return client.Id is 0 ? Results.BadRequest("Erreur d'ajout du client") : Results.Created("", client.IdPublic);
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<ClientImport> _validator,
        [FromServices] IClientService _clientServ,
        [FromBody] ClientImport _clientImport,
        [FromRoute(Name = "idPublicClient")] Guid _idPublicClient
    )
    {
        _clientImport.IdPublic = _idPublicClient;
        _clientImport.Mode = Enums.EModeImport.Modifier;

        var validate = await _validator.ValidateAsync(_clientImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        int idGroupe = _httpContext.RecupererIdGroupe();

        SetPropertyBuilder<Client> builder = new();

        builder.SetProperty(x => x.Adresse, _clientImport.Adresse.XSS())
            .SetProperty(x => x.Nom, _clientImport.Nom.XSS())
            .SetProperty(x => x.AdresseFacturation, string.IsNullOrWhiteSpace(_clientImport.AdresseFacturation) ? _clientImport.Adresse.XSS() : _clientImport.AdresseFacturation.XSS())
            .SetProperty(x => x.Mail, string.IsNullOrWhiteSpace(_clientImport.Mail) ? null : _clientImport.Mail)
            .SetProperty(x => x.Telephone, string.IsNullOrWhiteSpace(_clientImport.Telephone) ? null : _clientImport.Telephone.XSS())
            .SetProperty(x => x.InfoComplementaire, string.IsNullOrWhiteSpace(_clientImport.InfoComplementaire) ? null : _clientImport.InfoComplementaire.XSS());

        bool ok = await _clientServ.ModifierAsync(builder, idGroupe, _idPublicClient);

        return ok ? Results.NoContent() : Results.NotFound();
    }
}
