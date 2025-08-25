namespace Api.test;
public class TvaTest
{
    string baseUrl = "api/tva";

    [Theory]
    [InlineData("/lister", "GET")]
    public async Task Pas_acces_tva(string _url, string _httpMethod)
    {
        Connexion.HttpClient.DefaultRequestHeaders.Authorization = null;

        var reponse = await Connexion.HttpClient.SendAsync(new HttpRequestMessage()
        {
            Method = Connexion.VerbeHttp(_httpMethod),
            RequestUri = new Uri($"{baseUrl}{_url}", UriKind.RelativeOrAbsolute),
        });

        Assert.Equal(HttpStatusCode.Unauthorized, reponse.StatusCode);
    }

    [Fact]
    public async Task Lister_TVA()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/lister");

        Assert.True(reponse.IsSuccessStatusCode);
    }
}
