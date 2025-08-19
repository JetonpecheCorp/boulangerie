using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.test;

public class CommandeTest
{
    string baseUrl = "api/commande";

    [Theory]
    [InlineData("2020-01-01", "2020-12-31", 0, HttpStatusCode.OK)]
    [InlineData("2020-01-01", "2020-12-31", 2, HttpStatusCode.OK)]
    [InlineData("2021-01-01", "2020-12-31", 0, HttpStatusCode.BadRequest)]
    public async Task Lister_commande(string _dateDebut, string _dateFin, int _status, HttpStatusCode _codeRetour)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/lister?dateFin={_dateFin}&dateDebut={_dateDebut}&status={_status}"
        );
            
        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Theory]
    [InlineData("Gup447390279310", HttpStatusCode.OK)]
    [InlineData("azer", HttpStatusCode.NotFound)]
    [InlineData("", HttpStatusCode.NotFound)]
    [InlineData(null, HttpStatusCode.NotFound)]
    public async Task Facture_commande(string _numero, HttpStatusCode _codeRetour)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.GetAsync(
            $"{baseUrl}/facture/{_numero}"
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Theory]
    [InlineData("0a1ea0c8-c898-4c54-8492-44d19b4ebcae", "2025-07-12", 5, HttpStatusCode.Created)]
    [InlineData("", "", -5, HttpStatusCode.BadRequest)]
    [InlineData("0a1ea0c8-c898-4c54-8492-44d19b4e", "2025-01-01", 5, HttpStatusCode.BadRequest)]
    [InlineData("0a1ea0c8-c898-4c54-8492-44d19b4ebcae", "2020/01/01", 5, HttpStatusCode.BadRequest)]
    public async Task Ajouter_commande(string _idPublicClient, string _date, int _qte, HttpStatusCode _codeRetour)
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new CommandeRequete(
                _idPublicClient,
                _date,
                [
                    new ("c8b114a3-8ab6-485d-9aef-b864371f8504", _qte)
                ]
            )
        );

        Assert.Equal(reponse.StatusCode, _codeRetour);
    }

    [Fact]
    public async Task Modifier_status_commande()
    {
        await Connexion.AdministrateurAsync();

        var reponse = await Connexion.HttpClient.PostAsJsonAsync(
            $"{baseUrl}/ajouter", new CommandeRequete(
                "0a1ea0c8-c898-4c54-8492-44d19b4ebcae",
                "2025-08-12",
                [
                    new ("c8b114a3-8ab6-485d-9aef-b864371f8504", 5)
                ]
            )
        );

        string numero = (await reponse.Content.ReadFromJsonAsync<string>())!;

        int[] listeStatus = [0, 1, 2, 3, 4];
        foreach (var element in listeStatus)
        {
            reponse = await Connexion.HttpClient.PutAsJsonAsync(
                $"{baseUrl}/modifierStatus", new CommandeModifierStatusRequete(numero, element)
            );

            if(element is 1 or 4)
                Assert.Equal(HttpStatusCode.InternalServerError, reponse.StatusCode);

            else if(element is 3)
                Assert.Equal(HttpStatusCode.NotFound, reponse.StatusCode);

            else
                Assert.Equal(HttpStatusCode.NoContent, reponse.StatusCode);
        }
    }
}

public record CommandeModifierStatusRequete(string Numero, int Status);

public record ProduitCommandeRequete(string IdPublic, int Quantite);
public record CommandeRequete(string IdPublicClient, string Date, ProduitCommandeRequete[] ListeProduit);
