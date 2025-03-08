using System.Text.RegularExpressions;

namespace Api.Extensions;

public static class StringExtension
{
    public static string XSS(this string _valeur) => Regex.Replace(_valeur, "<[^>]*>", "", RegexOptions.Compiled);

    public static bool MailInvalide(this string _mail)
    {
        return !Regex.IsMatch(
            _mail, 
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-z]{2,4}|[0-9]{1,3})(\]?)$", 
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );
    }
}
