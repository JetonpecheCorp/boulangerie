export type IngredientExport =
{
    /** Pour la modification */
    idPublic?: string,

    nom: string,
    codeInterne?: string,
    stock: number,
    stockAlert: number
}