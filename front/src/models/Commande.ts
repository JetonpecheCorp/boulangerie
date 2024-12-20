export type Commande = 
{
    numero: string,
    date: Date,
    estLivraison: boolean,
    
    client: {
        idPublic: string,
        nom: string
    },
    listeProduit: ProduitCommande[]
};

export type ProduitCommande =
{
    idPublic: string,
    nom: string
    quantite: number
};