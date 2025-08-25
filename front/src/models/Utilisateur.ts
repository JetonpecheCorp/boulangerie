export type UtilisateurConnecter =
{
    nom: string,
    prenom: string,
    jwt: string
}

export type Utilisateur =
{
    idPublic: string,
    nom: string,
    prenom: string,
    mail: string,
    telephone?: string,
    estAdmin: boolean
}

export type UtilisateurLeger =
{
    idPublic: string,
    nomComplet: string
}