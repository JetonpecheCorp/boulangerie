using Api.Extensions;
using CsvHelper.Configuration;

namespace Api.ModelsImports.CSVs;

public sealed record ClientCSV
{
    public required string Nom { get; init; }
    public required string Adresse { get; init; }
    public required string AdresseFacturation { get; init; }
    public string? Mail { get; init; }
    public string? Telephone { get; init; }
}

public class ClientCsvMap : ClassMap<ClientCSV>
{
    public ClientCsvMap()
    {
        Map(x => x.Nom).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("nom *");
        Map(x => x.Adresse).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("adresse *");
        Map(x => x.AdresseFacturation).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("adresse de facturation *");
        Map(x => x.Mail)
            .Optional()
            .Validate(x => string.IsNullOrWhiteSpace(x.Field) || !x.Field.MailInvalide(), x => "Se n'est pas une adresse mail")
            .Name("mail")
            .Default(null);

        Map(x => x.Telephone).Optional().Name("téléphone").Default(null);

    }
}
