using System.Net.Http.Json;

namespace Api.test;

public static class Connexion
{
    public static HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5000/api")
    };

    static bool estConnecter = false;

    public static async Task<bool> AdministrateurAsync()
    {
        if (estConnecter)
            return true;

        var reponse = await HttpClient.PostAsJsonAsync("authentification", new ConnexionLog(
            "nicolas.np63@gmail.com",
            "Azertyuiop1&"
        ));

        if(reponse.IsSuccessStatusCode)
        {
            estConnecter = true;
            ConnexionLogReponse t = (await reponse.Content.ReadFromJsonAsync<ConnexionLogReponse>())!;

            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", t.Jwt);

            return true;
        }

        return false;
    }
}

file record ConnexionLog(string Login, string Mdp);
file record ConnexionLogReponse(string Jwt);
