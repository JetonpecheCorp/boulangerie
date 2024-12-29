import { ProduitCommandeExport } from "./ProduitCommandeExport"

export type CommandeExport =
{
    date: string,
    idPublicClient?: string,
    listeProduit: ProduitCommandeExport[]
}