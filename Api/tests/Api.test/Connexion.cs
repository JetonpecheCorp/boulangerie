using System.Net.Http;
using System.Net.Http.Json;

namespace Api.test;

public static class Connexion
{
    public static HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5000")
    };

    static bool estConnecter = false;

    public static async Task<bool> AdministrateurAsync()
    {
        if (estConnecter)
            return true;

        var reponse = await HttpClient.PostAsJsonAsync("api/authentification/connexion", new ConnexionLog(
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

    public static HttpMethod VerbeHttp(string _httpMethode)
    {
        return _httpMethode switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
    }
}

file record ConnexionLog(string Login, string Mdp);
file record ConnexionLogReponse(string Jwt);
