namespace Api.test;

public class UtilisateurTest
{
    string baseUrl = "api/utilisateur";

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_utilisateur(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData(1, 10, null)]
    [InlineData(-1, 10, "")]
    [InlineData(1, -10, "salut")]
    public async Task Lister_leger_utilisateur(int _numPage, int _nbParPage, string? _thermeRecherche = null)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/listerLeger?NumPage={_numPage}&NbParPage={_nbParPage}&ThermeRecherche={_thermeRecherche}"
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }

    [Theory]
    [InlineData("nom 1", "prenom 1", "Azertyuiop1&", "ff@a.com", "0123456789", HttpStatusCode.Created)]
    [InlineData("nom 1", "prenom 1", "azerty&", "fw@a.com", null, HttpStatusCode.BadRequest)]
    [InlineData("", "", "azerty&", "", null, HttpStatusCode.BadRequest)]
    public async Task Ajouter_utilisateur(
        string _nom,
        string _prenom,
        string _mdp,
        string _mail,
        string? _telephone,
        HttpStatusCode _codeRetour
    )
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new UtilisateurRequete(
                _nom,
                _prenom,
                _mdp,
                _mail,
                _telephone
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_utilisateur()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new UtilisateurRequete("utilisateur 222", "prenom 222", "Azertyuiop1&", "nn@x.com", null)
        );

        string idPublique = (await reponse.Content.ReadFromJsonAsync<string>())!;

        reponse = await Connexion.HttpClient.PutAsJsonAsync(
            $"{baseUrl}/modifier/{idPublique}", new UtilisateurModifierRequete("utilisateur modif", "prenom modif", "nn@x.com", null)
        );

        Assert.True(reponse.IsSuccessStatusCode);
    }
}

public record UtilisateurRequete(string Nom, string Prenom, string Mdp, string Mail, string? Telephone);
public record UtilisateurModifierRequete(string Nom, string Prenom, string Mail, string? Telephone);
