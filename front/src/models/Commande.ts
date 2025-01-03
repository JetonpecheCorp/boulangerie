export type Commande = 
{
    numero: string,
    date: Date,
    estLivraison: boolean,
    
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