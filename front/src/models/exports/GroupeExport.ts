import { Groupe } from "@model/Groupe";

export type GroupeExport = Omit<Groupe, "id" | "connexionBloquer">;