namespace Api.test;

public class IngredientTest
{
    string baseUrl = "api/ingredient";

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_ingredient(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("ingredient 1", null, 0, 10, HttpStatusCode.Created)]
    [InlineData("ingredient 1", null, 0, -10, HttpStatusCode.BadRequest)]
    [InlineData("ingredient 1", "aze", -1, 0, HttpStatusCode.BadRequest)]
    public async Task Ajouter_ingredient(
        string _nom,
        string? _codeInterne,
        decimal _stock,
        decimal _stockAlert,
        HttpStatusCode _codeRetour
    )
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new IngredientRequete(
                null,
                _nom,
                _codeInterne,
                _stock,
                _stockAlert
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_ingredient()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new IngredientRequete(
                null,
                "ingredient 10",
                "123",
                0, 
                0
            )
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier", new IngredientRequete(idPublique, "ingredient modif", "", 1, 10)
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

public record IngredientRequete(string? IdPublic, string Nom, string? CodeInterne, decimal Stock, decimal StockAlert);
