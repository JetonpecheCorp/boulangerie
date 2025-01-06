export type LivraisonExport =
{
    /** id public de l'utilisateur */
    idPublicConducteur: string,
    idPublicVehicule: string,

    /** format ISO AAAA-MM-JJ */
    date: string,
    liste: LivraisonCommandeExport[]
}

export type LivraisonCommandeExport =
{
    numero: string,
    ordre: number
}