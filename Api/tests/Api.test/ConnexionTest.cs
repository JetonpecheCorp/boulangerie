using System.Threading.Tasks;

namespace Api.test;

public class ConnexionTest
{
    [Fact]
    public async Task Test1()
    {
       Assert.True(await Connexion.AdministrateurAsync());
    }
}