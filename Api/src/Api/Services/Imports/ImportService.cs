using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsImports.CSVs;
using Api.Services.Utilisateurs;
using CsvHelper;
using CsvHelper.Configuration;
using Services.Mdp;
using System.Globalization;

namespace Api.Services.Imports;

public class ImportService(IUtilisateurService _utilisateurServ, IMdpService _mdpServ) : IImportService
{
    public List<ErreurValidationCSV> ListeErreur { get; private set; } = [];

    public async Task<List<ErreurValidationCSV>> UtilisateurAsync(int _idGroupe, IFormFile _fichierCSV)
    {
        using StreamReader lecteur = new(_fichierCSV.OpenReadStream());

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = System.Text.Encoding.UTF8,
            Delimiter = ";",
            ReadingExceptionOccurred = x =>
            {
                ListeErreur.Add(new()
                {
                    NumeroLigne = x.Exception.Context!.Parser!.Row,
                    NomHeader = x.Exception.Context.Reader!.HeaderRecord![x.Exception.Context.Reader.CurrentIndex],
                    Message = x.Exception.Message.Split(Environment.NewLine)[0],
                });

                return false;
            }
        };

        using (var csv = new CsvReader(lecteur, config))
        {
            csv.Context.RegisterClassMap<UtilisateurCsvMap>();
            var record = csv.GetRecordsAsync<UtilisateurCSV>();

            List<Utilisateur> liste = [];

            await foreach (var element in record)
            {
                liste.Add(new()
                {
                    EstAdmin = element.EstAdmin,
                    Nom = element.Nom.XSS(),
                    Prenom = element.Prenom.XSS(),
                    Mail = element.Mail,
                    IdGroupe = _idGroupe,
                    Telephone = element.Telephone?.XSS(),
                    IdPublic = Guid.NewGuid(),
                    Mdp = _mdpServ.Hasher(element.Mdp)
                });
            }

            await _utilisateurServ.AjouterAsync(liste);
        }

        return ListeErreur;
    }
}
