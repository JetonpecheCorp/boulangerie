import { EStatusCommande } from "@enum/EStatusCommande"

export type LivraisonLivreur =
{
    idPublic: string,
    vehicule: LivraisonLivreurVehicule,
    listeCommande: LivraisonCommande[] 
}

export type LivraisonLivreurVehicule =
{
    nom: string,
    immatriculation: string
}

export type LivraisonCommande = 
{
    numero: string,
    status: EStatusCommande,
    nomStatus: string,
    
    client: LivraisonLivreurClient | null,
    listeProduit: livraisonLivreurCommande[]
}

export type LivraisonLivreurClient =
{
    nom: string,
    adresse: string
}

export type livraisonLivreurCommande =
{
    nom: string,
    quantite: number
}
