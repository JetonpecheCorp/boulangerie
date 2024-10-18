using MailKit.Net.Smtp;
using MimeKit;
using System.Text.RegularExpressions;

namespace Services.Mail;
public sealed class MailService : IMailService
{
    private MailOptions mailOptions;
    public MailOptions MailOptions
    {
        private get
        {
            if (mailOptions is null)
                throw new ArgumentNullException($"'{nameof(MailOptions)}' ne peut pas être null");

            return mailOptions;
        }
        init
        {
            if (value is null)
                throw new ArgumentNullException($"'{nameof(MailOptions)}' ne peut pas être null");

            mailOptions = value;
        }
    }

    public MailService(MailOptions _mailOptions)
    {
        if (_mailOptions is null)
            throw new ArgumentNullException($"'{nameof(MailOptions)}' ne peut pas être null");

        MailOptions = _mailOptions;
    }

    public async Task<bool> EnvoyerAsync(IReadOnlyList<string> _listeDestinataire, string _sujet, string _message, bool _estFormatHtml)
    {
        if (_listeDestinataire is null || _listeDestinataire.Count is 0 || string.IsNullOrWhiteSpace(_message) || string.IsNullOrWhiteSpace(_sujet))
            return false;

        BodyBuilder bodyBuilder = new();

        if (_estFormatHtml)
            bodyBuilder.HtmlBody = _message;
        else
            bodyBuilder.TextBody = _message;

        using MimeMessage mimeMsg = new()
        {
            Subject = _sujet,
            Body = bodyBuilder.ToMessageBody()
        };

        ParametrerHeader(mimeMsg, _listeDestinataire, null, null);

        return await EnvoyerReelementAsync(mimeMsg);
    }

    public async Task<bool> EnvoyerAsync(IReadOnlyList<string> _listeDestinataire, string _sujet, string _message, bool _estFormatHtml, IReadOnlyList<string>? _listeCopie = null, IReadOnlyList<string>? _listeCopieCacher = null)
    {
        if (_listeDestinataire is null || _listeDestinataire.Count is 0 || string.IsNullOrWhiteSpace(_message) || string.IsNullOrWhiteSpace(_sujet))
            return false;

        BodyBuilder bodyBuilder = new();

        if (_estFormatHtml)
            bodyBuilder.HtmlBody = _message;
        else
            bodyBuilder.TextBody = _message;

        using MimeMessage mimeMsg = new()
        {
            Subject = _sujet,
            Body = bodyBuilder.ToMessageBody()
        };

        ParametrerHeader(mimeMsg, _listeDestinataire, _listeCopie, _listeCopieCacher);

        bool retour = await EnvoyerReelementAsync(mimeMsg);

        return retour;
    }

    private void ParametrerHeader(MimeMessage _mimeMsg, IReadOnlyList<string> _listeDestinataire, IReadOnlyList<string>? _listeCopie, IReadOnlyList<string>? _listeCopieCacher)
    {
        _mimeMsg.From.Add(new MailboxAddress("Expediteur", MailOptions.Expediteur));

        _listeDestinataire = FiltrerFauxMail(_listeDestinataire);

        foreach (string element in _listeDestinataire)
            _mimeMsg.To.Add(new MailboxAddress("Destinataire", element));

        if (_listeCopie is not null && _listeCopie.Count is not 0)
        {
            _listeCopie = FiltrerFauxMail(_listeCopie);

            foreach (string element in _listeCopie)
                _mimeMsg.Cc.Add(new MailboxAddress("Copie", element));
        }

        if (_listeCopieCacher is not null && _listeCopieCacher.Count is not 0)
        {
            _listeCopieCacher = FiltrerFauxMail(_listeCopieCacher);

            foreach (string element in _listeCopieCacher)
                _mimeMsg.Bcc.Add(new MailboxAddress("Copie cachée", element));
        }
    }

    private async Task<bool> EnvoyerReelementAsync(MimeMessage _mimeMsg)
    {
        try
        {
            using SmtpClient smtp = new()
            {
                CheckCertificateRevocation = false
            };

            await smtp.ConnectAsync(MailOptions.NomSmtp, MailOptions.NumeroPortSmtp);
            await smtp.AuthenticateAsync(MailOptions.Expediteur, MailOptions.Mdp);
            await smtp.SendAsync(_mimeMsg);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return false;
        }
    }

    private IReadOnlyList<string> FiltrerFauxMail(IReadOnlyList<string> _liste)
    {
        return _liste.Where(x => Regex.IsMatch(x, "^[a-z0-9-._]+@[a-z-]+.[a-z]{2,4}$")).ToList();
    }
}
