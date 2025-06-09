namespace Api.test;

public class FournisseurTest
{
    string baseUrl = "fournisseur";

    [Fact]
    public async Task Lister_fournisseur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/lister");

        Assert.True(reponse.IsSuccessStatusCode);
    }
}
