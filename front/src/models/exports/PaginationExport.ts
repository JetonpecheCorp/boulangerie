import { EStatusCommande } from "@enum/EStatusCommande"

export type PaginationExport =
{
    numPage: number,
    nbParPage: number,
    thermeRecherche?: string
}

export type PaginationFiltreLivraisonExport = PaginationExport & 
{
    dateDebut?: Date | null,
    dateFin?: Date | null,

    idPublicClient?: string | null
    idPublicConducteur?: string | null
}

export type CommandeFiltreExport = PaginationExport & 
{
    dateDebut: Date,
    dateFin: Date,

    status: EStatusCommande,
    sansLivraison?: boolean,
    idPublicClient?: string
}