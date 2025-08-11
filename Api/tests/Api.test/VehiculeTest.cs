namespace Api.test;

public class VehiculeTest
{
    string baseUrl = "api/vehicule";

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_vehicule(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("Toyata", "AA-123-AA", null, HttpStatusCode.Created)]
    [InlineData("Toyata", "AA-123-AA", "aze", HttpStatusCode.Created)]
    [InlineData("", "AA-123-AA", "aze", HttpStatusCode.BadRequest)]
    [InlineData("Triumph", null, null, HttpStatusCode.BadRequest)]
    public async Task Ajouter_vehicule(
        string _nom,
        string _immatriculation,
        string? _infoComplementaire,
        HttpStatusCode _codeRetour
    )
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new VehiculeRequete(
                _nom,
                _immatriculation,
                _infoComplementaire
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_vehicule()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new VehiculeRequete(
                "Voiture 1",
                "ZZ-222-QQ",
                null
            )
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idPublique}", new VehiculeRequete("voiture modif", "DD-898-XX", "AA")
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

public record VehiculeRequete(string Nom, string? Immatriculation, string? InfoComplementaire);
