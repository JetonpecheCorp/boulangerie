import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Commande } from '@model/Commande';
import { CommandeExport, CommandeFiltreExport } from '@model/exports/CommandeExport';
import { ConvertionEnum, EStatusCommande } from '../enums/EStatusCommande';

export class CommandeService 
{
  private readonly BASE_API = `${environment.urlApi}/commande`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_filtre: CommandeFiltreExport): Observable<Commande[]>
  { 
    let dateDebut = _filtre.dateDebut.toISOFormat();
    let dateFin = _filtre.dateFin.toISOFormat();

    const INFOS: any = { dateDebut, dateFin, status: _filtre.status };

    if(_filtre.sansLivraison != undefined && _filtre.sansLivraison != null)
      INFOS.sansLivraison = _filtre.sansLivraison ? "true" : "false"

    return this.http.get<Commande[]>(`${this.BASE_API}/lister`, { params: INFOS }).pipe(
        takeUntilDestroyed(this.destroyRef),
        map(listeCommande => 
        {
            for (let i = 0; i < listeCommande.length; i++) 
            {
              let element = listeCommande[i];
                
              element.date = new Date(element.date);
              element.nomStatus = ConvertionEnum.StatusCommande(element.status);
            }

            return listeCommande;
        }) 
    );
  }

  TelechargerFacture(_numero: string): void
  {
    this.http.get(`${this.BASE_API}/facture/${_numero}`,
      {observe: "response", responseType: "blob"}
    )
    .subscribe(reponse => 
    {
      const NOM_FICHIER = reponse.headers.get("content-disposition")!
        .replace("filename=", "")
        .split(";")[1];
      
      let a = document.createElement("a");
      a.download = NOM_FICHIER;
      let url = URL.createObjectURL(reponse.body as Blob);
      a.href = url;
      a.click();

      URL.revokeObjectURL(url);
    });
  }

  Ajouter(_commande: CommandeExport): Observable<string>
  {
    return this.http.post<string>(`${this.BASE_API}/ajouter`, _commande).pipe(takeUntilDestroyed(this.destroyRef));
  }

  modifierAdmin(_numeroCommande: string, _commande: CommandeExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifierAdmin/${_numeroCommande}`, _commande).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ModifierStatus(_numero: string, _status: EStatusCommande): Observable<void>
  {
    const INFOS = { 
      numero: _numero,
      status: _status
    };

    return this.http.put<void>(`${this.BASE_API}/modifierStatus`, INFOS).pipe(takeUntilDestroyed(this.destroyRef));
  }
}