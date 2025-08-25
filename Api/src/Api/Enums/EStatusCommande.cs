namespace Api.Enums;

public enum EStatusCommande
{
    Valider,
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
