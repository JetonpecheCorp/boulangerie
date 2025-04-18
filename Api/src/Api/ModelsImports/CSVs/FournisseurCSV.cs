using Api.Extensions;
using CsvHelper.Configuration;

namespace Api.ModelsImports.CSVs;

public sealed record FournisseurCSV
{
    public required string Nom { get; init; }
    public string? Adresse { get; init; }
    public string? Mail { get; init; }
    public string? Telephone { get; init; }
}

public class FournisseurCsvMap : ClassMap<FournisseurCSV>
{
    public FournisseurCsvMap()
    {
        Map(x => x.Nom).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("nom *");
        Map(x => x.Adresse).Optional().Name("adresse").Default(null);
        Map(x => x.Mail)
            .Optional()
            .Validate(x => string.IsNullOrWhiteSpace(x.Field) || !x.Field.MailInvalide(), x => "Se n'est pas une adresse mail")
            .Name("mail")
            .Default(null);

        Map(x => x.Telephone).Optional().Name("téléphone").Default(null);

    }
}
