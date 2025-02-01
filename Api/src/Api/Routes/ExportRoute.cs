using Api.Extensions;
using Api.Services.Clients;
using Api.Services.Utilisateurs;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Routes;

public static class ExportRoute
{
    private const string CONTENT_TYPE_EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public static RouteGroupBuilder AjouterRouteExport(this RouteGroupBuilder builder)
    {
        builder.MapGet("utilisateur", ExportUtilisateurAsync)
            .WithDescription("Produit un 'no content' si pas d'utilisateur")
            .Produces(StatusCodes.Status200OK, contentType: CONTENT_TYPE_EXCEL)
            .ProducesNoContent();

        builder.MapGet("client", ExportClientAsync)
            .WithDescription("Produit un 'no content' si pas d'utilisateur")
            .Produces(StatusCodes.Status200OK, contentType: CONTENT_TYPE_EXCEL)
            .ProducesNoContent();

        return builder;
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
            CONTENT_TYPE_EXCEL,
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
            CONTENT_TYPE_EXCEL,
            "export_client.xlsx"
        );
    }
}
