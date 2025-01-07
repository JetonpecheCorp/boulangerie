import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LivraisonExport } from '@model/exports/LivraisonExport';
import { Livraison, LivraisonDetail } from '@model/Livraison';
import { Pagination } from '@model/Pagination';
import { PaginationExport } from '@model/exports/PaginationExport';

export class LivraisonService 
{
  private readonly BASE_API = `${environment.urlApi}/livraison`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Livraison>>
  {
    return this.http.get<Pagination<Livraison>>(`${this.BASE_API}/lister`, { params: _pagination })
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        map(retour =>  
        {
          for (let element of retour.liste) 
            element.date = new Date(element.date);  

          return retour;
        })
      );
  }

  Detail(_idPublicLivraison: string): Observable<LivraisonDetail>
  {
    return this.http.get<LivraisonDetail>(`${this.BASE_API}/detail/${_idPublicLivraison}`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_livraison: LivraisonExport): Observable<string>
  {
    return this.http.post<string>(`${this.BASE_API}/ajouter`, _livraison).pipe(takeUntilDestroyed(this.destroyRef));
  }
}
