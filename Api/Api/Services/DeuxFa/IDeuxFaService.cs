namespace Services.DeuxFa;

public interface IDeuxFaService
{
    /// <summary>
    /// Generer en base64 le 2fa
    /// </summary>
    /// <param name="_titre">Titre du 2fa dans l'app (google auth, MS auth ...)</param>
    /// <param name="_labelle">Sous titre du 2fa dans l'app (google auth, MS auth ...)</param>
    /// <returns>2fa en base64 et cleSecret</returns>
    public Reponse2fa Generer(string _titre, string _labelle);

    /// <summary>
    /// Verifie la validité du code pour le 2fa
    /// </summary>
    /// <param name="_cleSecret">Cle secrete du 2fa</param>
    /// <param name="_code">Code reçu d'un app 2fa (Google, MS ...)</param>
    /// <returns>True => OK / False => mauvais code</returns>
    public bool Verifier(string _cleSecret, string _code);
}