import { Commande } from "./Commande"

export type Livraison =
{
    idPublic: string,
    numero: string,
    date: Date,
    fraisHT: number
}

export type LivraisonDetail =
{
    vehicule: LivraisonVehicule,
    conducteur: LivraisonConducteur,
    listeCommande: Commande[] 
}

export type LivraisonVehicule =
{
    idPublic: string,
    nom: string,
    immatriculation: string
}

export type LivraisonConducteur =
{
    idPublic: string,
    nomComplet: string
}

export type LivraisonAjoutReponse =
{
    numero: string,
    idPublic: string
}