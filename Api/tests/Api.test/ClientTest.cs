namespace Api.test;

public class ClientTest
{
    string baseUrl = "api/client";

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_client(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_leger_client(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/listerLeger?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("client 1", "adresse 1", "adresse F 1", "a@a.com", "0123456789", "coucou",  HttpStatusCode.Created)]
    [InlineData("client 1", "adresse 1", null, null, null, null, HttpStatusCode.Created)]
    [InlineData("", "adresse 1", null, null, null, null, HttpStatusCode.BadRequest)]
    [InlineData("client 1", "", null, null, null, null, HttpStatusCode.BadRequest)]
    public async Task Ajouter_client(
        string _nom, 
        string _adresse, 
        string? _adresseFacturation,
        string? _mail,
        string? _telephone,
        string? _infoComplementaire,
        HttpStatusCode _codeRetour
    )
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new ClientRequete(
                _nom,
                _adresse,
                _adresseFacturation,
                _mail,
                _telephone,
                _infoComplementaire
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_client()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new ClientRequete("client 1", "adresse 1", "adresse F 1", "a@a.com", "0123456789", "coucou")
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idPublique}", new ClientRequete("client 2", "adresse 1", null, "a@a.com", "0123456789", "coucou")
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

file record ClientRequete(
    string Nom,
    string Adresse,
    string? AdresseFacturation,
    string? Mail,
    string? Telephone,
    string? InfoComplementaire
);

