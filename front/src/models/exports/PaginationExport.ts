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