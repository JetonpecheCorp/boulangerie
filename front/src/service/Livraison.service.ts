import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LivraisonExport } from '@model/exports/LivraisonExport';
import { Livraison, LivraisonAjoutReponse, LivraisonDetail } from '@model/Livraison';
import { Pagination } from '@model/Pagination';
import { PaginationFiltreLivraisonExport } from '@model/exports/PaginationExport';

export class LivraisonService 
{
  private readonly BASE_API = `${environment.urlApi}/livraison`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_paginationFiltre: PaginationFiltreLivraisonExport): Observable<Pagination<Livraison>>
  {
    const INFOS: any = {
      nbParPage: _paginationFiltre.nbParPage,
      numPage: _paginationFiltre.numPage
    };

    if(_paginationFiltre.idPublicClient)
      INFOS.idPublicClient = _paginationFiltre.idPublicClient;

    if(_paginationFiltre.idPublicConducteur)
      INFOS.idPublicConducteur = _paginationFiltre.idPublicConducteur;

    if(_paginationFiltre.thermeRecherche)
      INFOS.thermeRecherche = _paginationFiltre.thermeRecherche;

    if(_paginationFiltre.dateDebut)
      INFOS.dateDebut = _paginationFiltre.dateDebut.toISOFormat();

    if(_paginationFiltre.dateFin)
      INFOS.dateFin = _paginationFiltre.dateFin.toISOFormat();

    return this.http.get<Pagination<Livraison>>(`${this.BASE_API}/lister`, { params: INFOS })
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        map(retour =>  
        {
          for (let element of retour.liste) 
          {
            let date = new Date(element.date);
            
            date.setHours(0, 0, 0, 0);
            element.date = new Date(date);  
          }

          return retour;
        })
      );
  }

  Detail(_idPublicLivraison: string): Observable<LivraisonDetail>
  {
    return this.http.get<LivraisonDetail>(`${this.BASE_API}/detail/${_idPublicLivraison}`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_livraison: LivraisonExport): Observable<LivraisonAjoutReponse>
  {
    return this.http.post<LivraisonAjoutReponse>(`${this.BASE_API}/ajouter`, _livraison).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_idPublicLivraison: string, _livraison: LivraisonExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier/${_idPublicLivraison}`, _livraison).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Supprimer(_idPublicLivraison: string): Observable<void>
  {
    return this.http.delete<void>(`${this.BASE_API}/supprimer/${_idPublicLivraison}`).pipe(takeUntilDestroyed(this.destroyRef));
  }
}
