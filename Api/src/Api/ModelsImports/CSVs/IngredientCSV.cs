using CsvHelper.Configuration;

namespace Api.ModelsImports.CSVs;

public sealed record IngredientCSV
{
    public required string Nom { get; init; }
    public string? CodeInterne { get; init; }
    public decimal Stock { get; init; }
    public decimal StockAlert { get; init; }
}

public class IngredientCsvMap : ClassMap<IngredientCSV>
{
    public IngredientCsvMap()
    {
        Map(x => x.Nom).Validate(x => !string.IsNullOrWhiteSpace(x.Field), x => "Le nom est obligatoire").Name("nom *");
        Map(x => x.Stock).Optional().Name("stock").Default(0);
        Map(x => x.Stock).Optional().Name("stock d'alerte").Default(0);
        Map(x => x.CodeInterne).Optional().Name("code interne").Default(null);
    }
}
