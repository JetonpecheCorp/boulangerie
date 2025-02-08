export type UtilisateurExport =
{
    nom: string,
    prenom: string,
    mail: string,
    telephone?: string,
    estAdmin: boolean,
    mdp: string
}

export type UtilisateurModifierExport = Omit<UtilisateurExport, "mdp">;