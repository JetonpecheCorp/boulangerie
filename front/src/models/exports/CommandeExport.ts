import { ProduitCommandeExport } from "./ProduitCommandeExport"

export type CommandeExport =
{
    date: string,
    listeProduit: ProduitCommandeExport[]
}