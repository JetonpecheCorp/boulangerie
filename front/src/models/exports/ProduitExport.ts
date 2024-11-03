export type ProduitExport =
{
    /** Pour la modification */
    idPublic?: string,

    nom: string,
    codeInterne?: string,
    prixHt: number,
    listeAllergene: string[],
    poids?: number,
    stock: number,
    stockAlert: number,
    idTva: number,
    idPublicCategorie: string
}