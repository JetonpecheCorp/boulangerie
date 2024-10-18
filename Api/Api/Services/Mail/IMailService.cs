namespace Services.Mail;

public interface IMailService
{
    Task<bool> EnvoyerAsync(IReadOnlyList<string> _listeDestinataire, string _sujet, string _message, bool _estFormatHtml);
    Task<bool> EnvoyerAsync(IReadOnlyList<string> _listeDestinataire, string _sujet, string _message, bool _estFormatHtml, IReadOnlyList<string>? _listeCopie = null, IReadOnlyList<string>? _listeCopieCacher = null);
}