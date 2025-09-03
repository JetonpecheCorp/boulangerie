using System.ComponentModel;

namespace Api.Enums;

public enum EStatusCommande
{
    Valider,

    [Description("En attente de validation")]
    EnAttenteValidation,
    Annuler,
    Livrer,
    Tout
}

public enum EStatusCommandeModifier
{
    Valider = 0,
    Annuler = 2,
    Livrer = 3
}
