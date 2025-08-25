import { Client } from "@model/Client";

export type ClientExport = Omit<Client, "idPublic">;