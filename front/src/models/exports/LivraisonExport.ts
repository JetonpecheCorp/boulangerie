export type LivraisonExport =
{
    /** id public de l'utilisateur */
    idPublicConducteur: string,
    idPublicVehicule: string,

    /** format ISO AAAA-MM-JJ */
    date: string,
    liste: LivraisonCommandeExport[],
    frais: number
}

export type LivraisonCommandeExport =
{
    numero: string,
    ordre: number
}