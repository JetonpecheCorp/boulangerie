namespace Api.test;

public class ExportTest
{
    string baseUrl = "api/export";

    [Fact]
    public async Task Exporter_utilisateur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/utilisateur");

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Exporter_client()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/client");

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("2020-10-01", "2020-12-31")]
    [InlineData("2020-10-45", "2020-12-31")]
    [InlineData("2020-10-01", "2020-12-45")]
    [InlineData("2020-10-01", "2010-12-10")]
    public async Task Exporter_commande(string _dateDebut, string _dateFin)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/commande?DateDebut={_dateDebut}&DateFin={_dateFin}&Status=0");

        if (reponse.IsSuccessStatusCode)
            Assert.True(true);

        else
            Assert.False(false);
    }

    [Fact]
    public async Task Exporter_produit()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/produit");

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Exporter_fournisseur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/fournisseur");

        Assert.True(reponse.IsSuccessStatusCode);
    }
}
