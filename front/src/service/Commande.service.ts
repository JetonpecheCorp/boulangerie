import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Commande } from '@model/Commande';
import { CommandeExport } from '@model/exports/CommandeExport';
import { ConvertionEnum, EStatusCommande } from '../enums/EStatusCommande';
import { CommandeFiltreExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';

export class CommandeService 
{
  private readonly BASE_API = `${environment.urlApi}/commande`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_filtre: CommandeFiltreExport): Observable<Pagination<Commande>>
  { 
    let dateDebut = _filtre.dateDebut.toISOFormat();
    let dateFin = _filtre.dateFin.toISOFormat();

    const INFOS: any = { 
      dateDebut, 
      dateFin, 
      nbParPage: _filtre.nbParPage,
      numPage: _filtre.numPage,
      status: _filtre.status, 
      thermeRecherche: _filtre.thermeRecherche,
      sansLivraison: _filtre.sansLivraison,
      idPublicClient: _filtre.idPublicClient ?? null
    };

    if(!_filtre.idPublicClient)
      delete INFOS.idPublicClient;

    if(_filtre.sansLivraison == null || _filtre.sansLivraison == undefined)
      delete INFOS.sansLivraison;

    if(!_filtre.thermeRecherche)
      delete INFOS.thermeRecherche;

    return this.http.get<Pagination<Commande>>(`${this.BASE_API}/lister`, { params: INFOS }).pipe(
        takeUntilDestroyed(this.destroyRef),
        map(commandePaginer => 
        {
            for (let i = 0; i < commandePaginer.liste.length; i++) 
            {
              let element = commandePaginer.liste[i];
                
              element.date = new Date(element.date);
              element.nomStatus = ConvertionEnum.StatusCommande(element.status);
            }

            return commandePaginer;
        })
    );
  }

  TelechargerFacture(_numero: string): void
  {
    this.http.get(`${this.BASE_API}/facture/${_numero}`,
      {observe: "response", responseType: "blob"}
    )
    .pipe(takeUntilDestroyed(this.destroyRef))
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

  Supprimer(_numeroCommande: string): Observable<void>
  {
    return this.http.delete<void>(`${this.BASE_API}/supprimer/${_numeroCommande}`).pipe(takeUntilDestroyed(this.destroyRef));
  }
}