using Api.Extensions;
using CsvHelper.Configuration;
using System.Net.Mail;

namespace Api.ModelsImports.CSVs;

public sealed record UtilisateurCSV
{
    public required string Nom { get; init; }
    public required string Prenom { get; init; }
    public required string Mail { get; init; }
    public required string Mdp { get; init; }
    public string? Telephone { get; init; }
    public bool EstAdmin { get; init; }
}

public class UtilisateurCsvMap: ClassMap<UtilisateurCSV>
{
    public UtilisateurCsvMap()
    {
        Map(x => x.Nom).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("nom *");
        Map(x => x.Prenom).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le prénom est obligatoire").Name("prénom *");

        Map(x => x.Mail)
            .Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le mail est obligatoire")
            .Validate(x => !x.Field.MailValide(), x => "Se n'est pas une adresse mail")
            .Name("mail *");

        Map(x => x.Mdp).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le mot de passe est obligatoire").Name("mot de passe *");
        Map(x => x.Telephone).Optional().Name("téléphone").Default(null);
        Map(x => x.EstAdmin).Optional().Name("admin").Default(false);
    }
}
