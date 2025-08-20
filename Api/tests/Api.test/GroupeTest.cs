namespace Api.test;

public class GroupeTest
{
    string baseUrl = "api/groupe";

    [Theory]
    [InlineData("/lister", "GET")]
    [InlineData("/ajouter", "POST")]
    [InlineData("/bloquer-debloquer/1", "PUT")]
    [InlineData("/modifier/1", "PUT")]
    public async Task Pas_acces_groupe(string _url, string _httpMethod)
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
    public async Task Lister_groupe()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync($"{baseUrl}/lister");

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("grp 1", "adresse 1 ", HttpStatusCode.Created)]
    [InlineData(null, null, HttpStatusCode.BadRequest)]
    [InlineData("", "aaa", HttpStatusCode.BadRequest)]
    [InlineData("grp 2", "", HttpStatusCode.BadRequest)]
    public async Task Ajouter_groupe(string _nom, string _adresse, HttpStatusCode _codeRetour)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new GroupeRequete(
                _nom, 
                _adresse
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Bloquer_debloquer_groupe()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new GroupeRequete(
                "groupe 1",
                "Adresse 1"
            )
        );

        int idGroupe = (await reponse.Content.ReadFromJsonAsync<int>())!;

        reponse = await Connexion.HttpClient.PutAsync(
            $"{baseUrl}/bloquer-debloquer/{idGroupe}", null
        );

        Assert.True(reponse.IsSuccessStatusCode);

        reponse = await Connexion.HttpClient.PutAsync(
            $"{baseUrl}/bloquer-debloquer/{idGroupe}", null
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Modifier_groupe()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new GroupeRequete(
                "groupe 1",
                "Adresse 1"
            )
        );

        int idGroupe = (await reponse.Content.ReadFromJsonAsync<int>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idGroupe}", new GroupeRequete("grp modif", "adresse modif")
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

public record GroupeRequete(string? Nom, string? Adresse);