namespace Api.test;

public class ConnexionTest
{
    [Fact]
    public async Task Connexion1()
    {
       Assert.True(await Connexion.AdministrateurAsync());
    }
}