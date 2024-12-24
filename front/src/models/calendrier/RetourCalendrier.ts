import { Commande } from "@model/Commande"

export type RetourCalendrierMois =
{
    listeCommande: Commande[],
    date: Date
}