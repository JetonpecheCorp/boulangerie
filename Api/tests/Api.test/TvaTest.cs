namespace Api.test;
public class TvaTest
{
    string baseUrl = "api/tva";

    [Fact]
    public async Task Lister_TVA()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/lister");

        Assert.True(reponse.IsSuccessStatusCode);
    }
}
