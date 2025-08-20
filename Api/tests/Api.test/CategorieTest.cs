using System.Text;
using System.Text.Json;

namespace Api.test;

public class CategorieTest
{
    string baseUrl = "api/categorie";

    [Theory]
    [InlineData("/listerPaginer?NumPage=1&NbParPage=10", "GET")]
    [InlineData("/lister", "GET")]
    [InlineData("/ajouter", "POST")]
    [InlineData("/modifier/00000000-0000-0000-0000-000000000000", "PUT")]
    public async Task Pas_acces_categorie(string _url, string _httpMethod)
    {
        Connexion.HttpClient.DefaultRequestHeaders.Authorization = null;

        var reponse = await Connexion.HttpClient.SendAsync(new HttpRequestMessage()
        {
            Method = Connexion.VerbeHttp(_httpMethod),
            RequestUri = new Uri($"{baseUrl}{_url}", UriKind.RelativeOrAbsolute),
        });

        Assert.Equal(HttpStatusCode.Unauthorized, reponse.StatusCode);
    }

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_paginer_categorie(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/listerPaginer?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Lister_categorie()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("cat 1", HttpStatusCode.Created)]
    [InlineData("", HttpStatusCode.BadRequest)]
    public async Task Ajouter_categorie(string _nom, HttpStatusCode _codeRetour)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new CategorieRequete(_nom)
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_categorie()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new CategorieRequete("salut")
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idPublique}", new CategorieRequete("coucou")
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

file record CategorieRequete(string Nom);
