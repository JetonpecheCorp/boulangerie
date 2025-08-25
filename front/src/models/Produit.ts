import { Categorie } from "./Categorie"
import { Tva } from "./Tva"

export type Produit = 
{
    idPublic: string,
    nom: string,
    codeInterne?: string,
    prixHt: number,
    listeAllergene: string[],
    poids?: number,
    stock: number,
    stockAlert: number,
    tva: Tva,
    categorie: Categorie
}

export type ProduitLeger = 
{
    idPublic: string,
    nom: string,
    prixHt: number
}