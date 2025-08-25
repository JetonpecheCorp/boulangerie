import { EStatusCommande } from "../enums/EStatusCommande";

export type Commande = 
{
    numero: string,
    date: Date,
    estLivraison: boolean,
    status: EStatusCommande,
    nomStatus: string,
    
    client: ClientCommande | null,
    livraison: CommandeLivraison | null,
    listeProduit: ProduitCommande[]
};

export type CommandeLivraison =
{
    idPublic: string,
    ordre: number
}

export type ProduitCommande =
{
    idPublic: string,
    nom: string,
    quantite: number
};

export type ClientCommande = 
{
    idPublic: string,
    nom: string,
    adresse: string
};