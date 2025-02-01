using Api.Extensions;
using Api.Services.Utilisateurs;
using ClosedXML.Excel;

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

        return builder;
    }

    async static Task<IResult> ExportUtilisateurAsync(
        HttpContext _httpContext,
        IUtilisateurService _utilisateurServ
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
}
