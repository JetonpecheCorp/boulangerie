namespace Api.test;

public class ConnexionTest
{
    [Fact]
    public async Task Connexion1()
    {
       Assert.True(await Connexion.AdministrateurAsync());
    }

    [Fact]
    public async Task Connnexion_refuser()
    {
        Connexion.HttpClient.DefaultRequestHeaders.Authorization = null;
        var reponse = await Connexion.HttpClient.PostAsJsonAsync("api/authentification/connexion", new ConnexionLog(
            "a@a.com",
            "Azertyuiop1&"
        ));

        Assert.Equal(HttpStatusCode.BadRequest, reponse.StatusCode);
    }
}

file record ConnexionLog(string Login, string Mdp);
