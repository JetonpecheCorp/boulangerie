export type Produit = 
{
    idPublic: string,
    nom: string,
    codeInterne?: string,
    prixHt: number,
    listeAllergene: string[],
    poids?: number,
    stock: number,
    stockAlert: number
}