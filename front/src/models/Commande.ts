export type Commande = 
{
    numero: string,
    date: Date,
    estLivraison: boolean,
    
    client: ClientCommande | null,
    listeProduit: ProduitCommande[]
};

export type ProduitCommande =
{
    idPublic: string,
    nom: string,
    quantite: number
};

export type ClientCommande = 
{
    idPublic: string,
    nom: string
};

export type ProduitCommandeExistant =
{
    idPublic: string,
    nom: string
}