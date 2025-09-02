import { EStatusCommande } from "../../enums/EStatusCommande"
import { ProduitCommandeExport } from "./ProduitCommandeExport"

export type CommandeExport =
{
    date: string,
    estLivraison: boolean,
    idPublicClient?: string,
    listeProduit: ProduitCommandeExport[]
}

export type CommandeFiltreExport =
{
    dateDebut: Date,
    dateFin: Date,
    status: EStatusCommande,
    sansLivraison?: boolean,
    idPublicLivraison?: string
}