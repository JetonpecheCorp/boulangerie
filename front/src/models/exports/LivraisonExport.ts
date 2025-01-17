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

export type LivraisonModifierExport = Omit<LivraisonExport, "date">;

export type LivraisonCommandeExport =
{
    numero: string,
    ordre: number
}