using Api.Extensions;
using Api.Models;
using Api.ModelsExports;
using Api.ModelsImports.CSVs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Services.Mdp;
using System.Globalization;
using System.Text;

namespace Api.Services.Imports;

public class ImportService(BoulangerieContext _context, IMdpService _mdpServ) : IImportService
{
    public List<ErreurValidationCSV> ListeErreur { get; private set; } = [];

    private CsvConfiguration Config => new(CultureInfo.InvariantCulture)
    {
        Encoding = Encoding.UTF8,
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

    public async Task<List<ErreurValidationCSV>> ClientAsync(int _idGroupe, IFormFile _fichierCSV)
    {
        using StreamReader lecteur = new(_fichierCSV.OpenReadStream());

        using (var csv = new CsvReader(lecteur, Config))
        {
            csv.Context.RegisterClassMap<ClientCsvMap>();
            var record = csv.GetRecords<ClientCSV>().ToArray();

            List<Client> liste = [];

            for(int i = 0; i < record.Length; i++)
            {
                var element = record[i];

                if(element.Mail is not null && await _context.Clients.AnyAsync(x => x.Mail == element.Mail))
                {
                    ListeErreur.Add(new()
                    {
                        Message = "Le mail existe déjà",
                        NomHeader = "mail",
                        NumeroLigne = i + 2
                    });

                    continue;
                }

                liste.Add(new()
                {
                    Nom = element.Nom.XSS(),
                    IdGroupe = _idGroupe,
                    IdPublic = Guid.NewGuid(),
                    Adresse = element.Adresse.XSS(),
                    AdresseFacturation = element.AdresseFacturation.XSS(),
                    Mail = element.Mail?.XSS(),
                    Telephone = element.Telephone?.XSS()
                });
            }

            _context.Clients.AddRange(liste);
            await _context.SaveChangesAsync();
        }

        return ListeErreur;
    }

    public async Task<List<ErreurValidationCSV>> IngredientAsync(int _idGroupe, IFormFile _fichierCSV)
    {
        using StreamReader lecteur = new(_fichierCSV.OpenReadStream());

        using (var csv = new CsvReader(lecteur, Config))
        {
            csv.Context.RegisterClassMap<IngredientCsvMap>();
            var record = csv.GetRecordsAsync<IngredientCSV>();

            List<Ingredient> liste = [];

            await foreach (var element in record)
            {
                liste.Add(new()
                {
                    Nom = element.Nom.XSS(),
                    IdGroupe = _idGroupe,
                    IdPublic = Guid.NewGuid(),
                    CodeInterne = element.CodeInterne?.XSS(),
                    Stock = element.Stock,
                    StockAlert = element.StockAlert
                });
            }

            _context.Ingredients.AddRange(liste);
            await _context.SaveChangesAsync();
        }

        return ListeErreur;
    }

    public async Task<List<ErreurValidationCSV>> UtilisateurAsync(int _idGroupe, IFormFile _fichierCSV)
    {
        using StreamReader lecteur = new(_fichierCSV.OpenReadStream());

        using (var csv = new CsvReader(lecteur, Config))
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

            _context.Utilisateurs.AddRange(liste);
            await _context.SaveChangesAsync();
        }

        return ListeErreur;
    }
}
