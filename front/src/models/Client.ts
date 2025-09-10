export type Client = 
{
    idPublic: string,
    nom: string,
    mail?: string,
    telephone?: string,
    adresse: string,
    adresseFacturation: string,
    infoComplementaire?: string,
    possedeCompte: boolean,
    connexionBloquer: boolean
};

export type ClientLeger =
{
    idPublic: string,
    nom: string,
    adresse: string
}