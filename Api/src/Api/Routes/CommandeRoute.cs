using Api.Constantes;
using Api.Extensions;
using Api.Extensions.PdfStyle;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsExports.Commandes;
using Api.ModelsImports.Commandes;
using Api.Services.Clients;
using Api.Services.Commandes;
using Api.Services.Groupes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Services.Mdp;

namespace Api.Routes;

public static class CommandeRoute
{
    public static RouteGroupBuilder AjouterRouteCommande(this RouteGroupBuilder builder)
    {
        builder.WithOpenApi().ProducesServiceUnavailable();

        builder.MapGet("lister", ListerAsync)
            .WithDescription("""
                Lister les commandes (Status: 0 => Valider, 1 => En attente de validation, 2 => Annuler, 3 => Livrer (terminer), 4 => Tout)  
                ATTENTION: Recherche prioritaire et non cumulable (numéro de commande)
            """)
            .ProducesBadRequest()
            .Produces<PaginationExport<CommandeExport>>();

        builder.MapGet("facture/{numero}", GenererFactureAsync)
            .WithDescription("Ajouter une nouvelle commande")
            .Produces(StatusCodes.Status200OK, contentType: ContentType.Pdf);

        builder.MapPost("ajouter", AjouterAsync)
            .WithDescription("Ajouter une nouvelle commande")
            .ProducesCreated<string>()
            .ProducesBadRequestErreurValidation();

        builder.MapPut("modifierAdmin/{numeroCommande}", ModifierAsync)
            .WithDescription("Modifier une commande")
            .ProducesBadRequestErreurValidation()
            .ProducesNoContent();

        builder.MapPut("modifierStatus", ModifierStatusAsync)
            .WithDescription("Modifier le status d'une commande")
            .ProducesNotFound()
            .ProducesNoContent();

        builder.MapDelete("supprimer/{numeroCommande}", SupprimerAsync)
            .WithDescription("Supprimer une commande")
            .ProducesBadRequest()
            .ProducesNotFound()
            .ProducesNoContent();

        return builder;
    }

    static async Task<IResult> GenererFactureAsync(
        HttpContext _httpContext,
        [FromServices] ICommandeService _commandeServ,
        [FromServices] IGroupeService _groupeServ,
        [FromRoute(Name = "numero")] string _numero
    )
    {
        if (string.IsNullOrWhiteSpace(_numero))
            return Results.BadRequest("pas de numero de commande");

        int idGroupe = _httpContext.RecupererIdGroupe();

        var commande = await _commandeServ.InfoAsync(_numero, idGroupe);
        var groupe = await _groupeServ.InfoAsync(idGroupe);

        if (commande is null || groupe is null)
            return Results.NotFound();

        var doc = Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(10);

                page.Header()
                    .BorderBottom(2)
                    .BorderColor(Colors.Black)
                    .Text("Facture")
                    .FontSize(25);

                page.Content().Column(x =>
                {
                    x.Spacing(10);

                    x.Item().Row(row =>
                    {
                        row.RelativeItem().Text($"""
                           Numéro de commande: {_numero}
                           {(commande.Client is null ? "" : "Adresse: " + commande.Client.Adresse)}   
                           {(commande.Client is null ? "" : "Nom: " + commande.Client.Nom)}
                           Date: {commande.Date.ToString("d")}
                           Livraison: {(commande.EstLivraison ? "Oui" : "Non")}
                        """);

                        row.RelativeItem().Text($"""
                            De:
                            Nom: {groupe.Nom}
                            Adresse: {groupe.Adresse}
                        """);
                    });

                    x.Item().Table(table =>
                    {
                        table.ColumnsDefinition(x =>
                        {
                            x.RelativeColumn();
                            x.RelativeColumn();
                            x.RelativeColumn();
                            x.RelativeColumn();
                            x.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            table.CellStyleHeaderTableau().Text("Produit");
                            table.CellStyleHeaderTableau().Text("Quantité");
                            table.CellStyleHeaderTableau().Text("Prix HT");
                            table.CellStyleHeaderTableau().Text("TVA");
                            table.CellStyleHeaderTableau().Text("Total HT");
                        });

                        decimal totalHt = 0;
                        decimal totalTtc = 0;
                        uint nbLigne = (uint)commande.ListeProduit.Length + 2;

                        foreach (var element in commande.ListeProduit)
                        {
                            totalHt += element.Quantite * element.PrixHT;

                            decimal tvaParUnite = element.PrixHT * (element.Tva / 100);
                            totalTtc += (element.PrixHT + tvaParUnite) * element.Quantite;

                            table.CellStyle().Text(element.Nom);
                            table.CellStyle().Text(element.Quantite.ToString());
                            table.CellStyle().Text($"{element.PrixHT} €");
                            table.CellStyle().Text($"{element.Tva} %");
                            table.CellStyle().Text($"{element.Quantite * element.PrixHT} €");
                        }

                        table.Cell().Row(nbLigne).Column(4)
                            .Border(1)
                            .Background(Colors.Grey.Lighten3)
                            .BorderColor(Colors.Black)
                            .PaddingVertical(5)
                            .PaddingHorizontal(10)
                            .Text("Total HT");

                        table.Cell().Row(nbLigne)
                            .Column(5).Border(1)
                            .PaddingVertical(5)
                            .PaddingHorizontal(10)
                            .BorderColor(Colors.Black)
                            .Text($"{totalHt} €");

                        table.Cell().Row(nbLigne + 1).Column(4)
                            .Border(1)
                            .Background(Colors.Grey.Lighten3)
                            .BorderColor(Colors.Black)
                            .PaddingVertical(5)
                            .PaddingHorizontal(10)
                            .Text("Total TTC");

                        table.Cell().Row(nbLigne + 1)
                            .Column(5).Border(1)
                            .PaddingVertical(5)
                            .PaddingHorizontal(10)
                            .BorderColor(Colors.Black)
                            .Text($"{string.Format("{0:F2}", totalTtc)} €");
                    });
                });
            });
        });

        return Results.File(
            doc.GeneratePdf(),
            "application/pdf",
            $"facture_commande.pdf"
        );
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

        if(_httpContext.RecupererRole() == "client")
        {
            _commandeFiltre.IdPublicClient = Guid.Parse(_httpContext.RecupererIdPublique());
            _commandeFiltre.RoleClient = true;
        }

        var liste = await _commandeServ.ListerAsync(_commandeFiltre, idGroupe);

        return Results.Extensions.OK(liste, PaginationExportContext.Default);
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

        Guid idPublicClient = _commandeImport.IdPublicClient ?? Guid.Empty;

        int idGroupe = _httpContext.RecupererIdGroupe();
        string prefixGrp = await _groupeServ.PrefixAsync(idGroupe);
        int idClient = await _clientServ.RecupererIdAsync(idPublicClient, idGroupe);
        string numero = _mdpServ.Generer(12, false);

        Commande commande = new()
        {
            IdClient = idClient == 0 ? null : idClient,
            IdGroupe = idGroupe,
            Numero = $"{prefixGrp}{numero}",
            EstLivraison = _commandeImport.EstLivraison,
            DatePourLe = _commandeImport.Date.ToDateTime(TimeOnly.MinValue)
        };

        if(idClient is 0)
            commande.DateValidation = DateTime.UtcNow;

        bool retour = await _commandeServ.AjouterAsync(commande, _commandeImport.ListeProduit);

        return retour ? Results.Created("", commande.Numero) : Results.BadRequest("Erreur d'ajout");
    }

    async static Task<IResult> ModifierAsync(
        HttpContext _httpContext,
        [FromServices] IValidator<CommandeImport> _validator,
        [FromServices] ICommandeService _commandeServ,
        [FromRoute(Name = "numeroCommande")] string _numeroCommande,
        [FromBody] CommandeImport _commandeImport
    )
    {
        var validate = await _validator.ValidateAsync(_commandeImport);

        if (!validate.IsValid)
            return Results.Extensions.ErreurValidator(validate.Errors);

        bool ok = await _commandeServ.ModifierAsync(_numeroCommande, _commandeImport);

        return ok ? Results.NoContent() : Results.BadRequest();
    }

    async static Task<IResult> ModifierStatusAsync(
        HttpContext _httpContext,
        [FromServices] ICommandeService _commandeServ,
        [FromBody] CommandeModifierStatusImport _commande
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        bool ok = await _commandeServ.ModifierStatusAsync(_commande.Numero, _commande.Status, idGroupe);

        return ok ? Results.NoContent() : Results.NotFound();
    }

    async static Task<IResult> SupprimerAsync(
        HttpContext _httpContext,
        [FromServices] ICommandeService _commandeServ,
        [FromRoute(Name = "numeroCommande")] string _numeroCommande
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var etat = await _commandeServ.SupprimerAsync(_numeroCommande, idGroupe);

        return etat switch
        {
            EReponseSupprimerCommande.Ok => Results.NoContent(),
            EReponseSupprimerCommande.ExistePas => Results.NotFound(),
            EReponseSupprimerCommande.PeutPasEtreSupprimer => Results.BadRequest("La commande ne peut pas être supprimée"),
           _ => Results.NoContent()
        };

    }
}
