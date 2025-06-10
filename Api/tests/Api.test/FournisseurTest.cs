using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace Api.test;

public class FournisseurTest
{
    string baseUrl = "api/fournisseur";

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_fournisseur(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("nom", "adresse 1", "0123456789", "w@s.com", HttpStatusCode.Created)]
    [InlineData("nom", null, "0123456789", "w@s.com", HttpStatusCode.Created)]
    [InlineData("nom", null, null, "w@s.com", HttpStatusCode.Created)]
    [InlineData("nom", null, null, null, HttpStatusCode.Created)]
    [InlineData("", "adresse 1", "0123456789", "w@s.com", HttpStatusCode.BadRequest)]
    public async Task Ajouter_fournisseur(
        string _nom, 
        string? _adresse, 
        string? _telephone, 
        string? _mail,
        HttpStatusCode _codeRetour
    )
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new FounisseurRequete(
                _nom,
                [],
                [],
                _adresse,
                _telephone,
                _mail
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_fournisseur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new FounisseurRequete(
                "nom test",
                [],
                [],
                null,
                null,
                null
            )
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idPublique}", new FounisseurRequete(
                "coucou",
                [],
                [],
                "adresse test",
                "0612345678",
                "x@x.com"
            )
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Archiver_fournisseur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new FounisseurRequete(
                "nom test",
                [],
                []
            )
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.DeleteAsync($"{baseUrl}/archiver/{idPublique}");

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

file record FounisseurRequete(
    string Nom, 
    string[] ListeIdPublicIngredient,
    string[] ListeIdPublicProduit,
    string? Adresse = null, 
    string? Telephone = null, 
    string? Mail = null
);
