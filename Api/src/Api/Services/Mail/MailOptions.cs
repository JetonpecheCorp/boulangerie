using System.Text.RegularExpressions;

namespace Services.Mail;
public sealed class MailOptions
{
    private readonly string expediteur = null!;
    private readonly string nomSmtp = null!;
    private readonly string mdp = null!;

    /// <summary>
    /// Adresse mail de l'expéditeur
    /// </summary>
    public string Expediteur
    {
        get => expediteur;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{nameof(Expediteur)}' ne peut pas être null ou vide");

            if (!Regex.IsMatch(value, "^[a-z0-9-._]+@[a-z-]+.[a-z]{2,4}$"))
                throw new ArgumentException($"'{nameof(Expediteur)}' doit être un mail");

            expediteur = value;
        }
    }

    /// <summary>
    /// Mot de passe du mail de l'expéditeur
    /// </summary>
    public string Mdp
    {
        get => mdp;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{nameof(mdp)}' ne peut pas être null ou vide");

            mdp = value;
        }
    }

    /// <summary>
    /// Nom du SMTP (exemple: smtp.gmail.com)
    /// </summary>
    public string NomSmtp
    {
        get => nomSmtp;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"'{nameof(NomSmtp)}' ne peut pas être null ou vide");

            nomSmtp = value;
        }
    }

    /// <summary>
    /// Numero du port SMTP
    /// </summary>
    public ushort NumeroPortSmtp { get; init; }
}
