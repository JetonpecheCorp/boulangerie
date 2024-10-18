using TwoFactorAuthNet;

namespace Services.DeuxFa;
public sealed class DeuxFaService : IDeuxFaService
{
    public Reponse2fa Generer(string _titre, string _labelle)
    {
        if (string.IsNullOrWhiteSpace(_titre))
            _titre = "titre app";

        if (string.IsNullOrWhiteSpace(_labelle))
            _labelle = "Sous titre app";

        TwoFactorAuth tfa = new(_titre);

        // multiple de 8
        // 80 ou 160 defaut 80
        // 160 recommandé pour une meilleur sécurité
        string cleSecret = tfa.CreateSecret(160);
        string url2fa = tfa.GetQrText(_labelle, cleSecret);

        Reponse2fa reponse = new()
        {
            CleSecret = cleSecret,
            Url2fa = url2fa
        };

        return reponse;
    }

    public bool Verifier(string _cleSecret, string _code)
    {
        if (string.IsNullOrWhiteSpace(_code) || string.IsNullOrWhiteSpace(_cleSecret))
            return false;

        TwoFactorAuth tfa = new();

        return tfa.VerifyCode(_cleSecret, _code);
    }
}