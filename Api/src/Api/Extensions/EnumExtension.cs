using System.ComponentModel;
using System.Reflection;

namespace Api.Extensions;

public static class EnumExtension
{
    /// <summary>
    /// Personnaliser le nom de l'enum
    /// </summary>
    /// <param name="_enum"></param>
    /// <returns>Récupere la description dans <see cref="DescriptionAttribute"/> sinon le nom</returns>
    public static string Description(this Enum _enum)
    {
        var champ = _enum.GetType().GetField(_enum.ToString());
        var attribut = champ!.GetCustomAttribute<DescriptionAttribute>();

        return attribut is null ? _enum.ToString() : attribut.Description;
    }
}
