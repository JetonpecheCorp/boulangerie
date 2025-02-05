using Api.Constantes;
using Api.Extensions;
using Api.ModelsImports.Exports;
using Api.Services.Clients;
using Api.Services.Commandes;
using Api.Services.Utilisateurs;
using Api.Extensions.PdfStyle;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Text;

namespace Api.Routes;

public static class ExportRoute
{
    public static RouteGroupBuilder AjouterRouteExport(this RouteGroupBuilder builder)
    {
        builder.MapGet("utilisateur", ExportUtilisateurAsync)
            .WithDescription("Produit un 'no content' si pas d'utilisateur")
            .Produces(StatusCodes.Status200OK, contentType: ContentType.Excel)
            .ProducesNoContent();

        builder.MapGet("client", ExportClientAsync)
            .WithDescription("Produit un 'no content' si pas d'utilisateur")
            .Produces(StatusCodes.Status200OK, contentType: ContentType.Excel)
            .ProducesNoContent();

        builder.MapGet("commande", ExportCommandeAsync);

        return builder;
    }

    async static Task<IResult> ExportCommandeAsync(
        HttpContext _httpContext,
        [FromServices] ICommandeService _commandeServ,
        [AsParameters] DateIntervalImport _dateInterval
    )
    {
        if (_dateInterval.DateDebut > _dateInterval.DateFin)
            return Results.BadRequest("La date debut est plus grand que la date de fin");

        int idGroupe = _httpContext.RecupererIdGroupe();

        var listeCommande = await _commandeServ.ListerAsync(new ModelsImports.Commandes.CommandeFiltreImport
        {
            DateDebut = _dateInterval.DateDebut,
            DateFin = _dateInterval.DateFin,
            Status = _dateInterval.Status
        }, idGroupe);

        var listeCommandeGroupeBy = listeCommande.GroupBy(x => x.Date).ToArray();

        var doc = Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                
                page.Margin(10);

                page.Header()
                    .BorderBottom(2)
                    .BorderColor(Colors.Black)
                    .Text($"Commande du {_dateInterval.DateDebut} au {_dateInterval.DateFin}")
                    .FontSize(20);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(x =>
                    {
                        for (int i = 0; i < listeCommandeGroupeBy.Length; i++)
                            x.RelativeColumn();
                    });

                    var liste = listeCommandeGroupeBy.SelectMany(x => x).ToArray();

                    /**
                     * 6/12/2024 0
                     * cmd 1 row j = 1 colonne  i = 1
                     * cmd 2 row j = 2 colonne i = 1
                     * 
                     */

                    for (uint i = 0; i < listeCommandeGroupeBy.Length; i++)
                    {
                        var element = listeCommandeGroupeBy[i];
                        table.Cell()
                            .Row(0)
                            .Column(i + 1)
                            .StyleHeaderTableau()
                            .Text(element.Key.ToString("d"));

                        var listeCommande = element.Select(x => x).ToArray();

                        for (uint j = 0; j < listeCommande.Length; j++)
                        {
                            var element2 = listeCommande[j];

                            var listeStringProduit = new StringBuilder();
                            listeStringProduit.AppendLine(element2.Numero);

                            if (element2.Client is not null)
                            {
                                listeStringProduit.AppendLine(element2.Client.Nom);
                                listeStringProduit.AppendLine(element2.Client.Adresse);
                            }
                            
                            for (int k = 0; k < element2.ListeProduit.Length; k++)
                            {                  
                                var produit = element2.ListeProduit[k];

                                listeStringProduit.AppendLine($"{produit.Nom} X{produit.Quantite}");
                            }

                            table.Cell()
                                .Row(j + 2)
                                .Column(i + 1)
                                .Style()
                                .Text(listeStringProduit.ToString());
                        }
                    }
                });
            });
        });

        return Results.File(
            doc.GeneratePdf(),
            ContentType.Pdf,
            "planning.pdf"
        );
    }

    async static Task<IResult> ExportUtilisateurAsync(
        HttpContext _httpContext,
        [FromServices] IUtilisateurService _utilisateurServ
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var info = await _utilisateurServ.ListerAsync(
            new ModelsImports.PaginationImport { NbParPage = 1_000_000, NumPage = 1 }, 
            idGroupe
        );

        if (info.Liste.Length is 0)
            return Results.NoContent();

        using XLWorkbook workbook = new();
        var worksheet = workbook.AddWorksheet();

        worksheet.Cell(1, 1).Value = "Nom";
        worksheet.Cell(1, 2).Value = "Prenom";
        worksheet.Cell(1, 3).Value = "Mail";
        worksheet.Cell(1, 4).Value = "Téléphone";
        worksheet.Cell(1, 5).Value = "Admin";

        worksheet.Columns("A:E").AdjustToContents();

        int ligneIndex = 2;
        for (int i = 0; i < info.Liste.Length; i++)
        {
            var element = info.Liste[i];

            worksheet.Cell(ligneIndex, 1).Value = element.Nom;
            worksheet.Cell(ligneIndex, 2).Value = element.Prenom;
            worksheet.Cell(ligneIndex, 3).Value = element.Mail;
            worksheet.Cell(ligneIndex, 4).Value = element.Telephone;
            worksheet.Cell(ligneIndex, 5).Value = element.EstAdmin ? "Oui" : "Non";

            ligneIndex++;
        }

        using MemoryStream stream = new();

        workbook.SaveAs(stream);
        stream.Position = 0;

        return Results.File(
            stream.ToArray(),
            ContentType.Excel,
            "export_utilisateur.xlsx"
        );
    }

    async static Task<IResult> ExportClientAsync(
        HttpContext _httpContext,
        [FromServices] IClientService _clientServ
    )
    {
        int idGroupe = _httpContext.RecupererIdGroupe();

        var info = await _clientServ.ListerAsync(
            new ModelsImports.PaginationImport { NbParPage = 1_000_000, NumPage = 1 },
            idGroupe
        );

        if (info.Liste.Length is 0)
            return Results.NoContent();

        using XLWorkbook workbook = new();
        var worksheet = workbook.AddWorksheet();

        worksheet.Cell(1, 1).Value = "Nom";
        worksheet.Cell(1, 2).Value = "Adresse";
        worksheet.Cell(1, 3).Value = "Mail";
        worksheet.Cell(1, 4).Value = "Téléphone";
        worksheet.Cell(1, 5).Value = "Adresse de facturation";
        worksheet.Cell(1, 6).Value = "Infos complementaire";

        worksheet.Columns("A:E").AdjustToContents();
        worksheet.Column("F").Width = 100;

        int ligneIndex = 2;
        for (int i = 0; i < info.Liste.Length; i++)
        {
            var element = info.Liste[i];

            worksheet.Cell(ligneIndex, 1).Value = element.Nom;
            worksheet.Cell(ligneIndex, 2).Value = element.Adresse;
            worksheet.Cell(ligneIndex, 3).Value = element.Mail;
            worksheet.Cell(ligneIndex, 4).Value = element.Telephone;
            worksheet.Cell(ligneIndex, 5).Value = element.AdresseFacturation;

            var cellule = worksheet.Cell(ligneIndex, 6);
            cellule.Value = element.InfoComplementaire;
            cellule.Style.Alignment.WrapText = true;

            ligneIndex++;
        }

        using MemoryStream stream = new();

        workbook.SaveAs(stream);
        stream.Position = 0;

        return Results.File(
            stream.ToArray(),
            ContentType.Excel,
            "export_client.xlsx"
        );
    }
}
