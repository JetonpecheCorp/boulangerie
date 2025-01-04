export enum EStatusCommande
{
    Valider,
    EnAttenteValidation,
    Annuler,
    Livrer,
    Tout
}

export class ConvertionEnum
{
    public static StatusCommande(_status: EStatusCommande): string
    {
        switch (_status) 
        {
            case EStatusCommande.Valider:
                return "Valider"

            case EStatusCommande.Annuler:
                return "Annuler"

            case EStatusCommande.Livrer:
                return "Livrer"

            case EStatusCommande.EnAttenteValidation:
                return "A valider"

            case EStatusCommande.Tout:
                return "Tout"
        }
    }
}