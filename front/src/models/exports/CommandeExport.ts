import { ProduitCommandeExport } from "./ProduitCommandeExport"

export type CommandeExport =
{
    date: string,
    estLivraison: boolean,
    idPublicClient?: string,
    listeProduit: ProduitCommandeExport[]
}
