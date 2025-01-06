import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LivraisonExport } from '@model/exports/LivraisonExport';

export class LivraisonService 
{
  private readonly BASE_API = `${environment.urlApi}/livraison`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Ajouter(_livraison: LivraisonExport): Observable<any>
  {
    return this.http.post<any>(`${this.BASE_API}/ajouter`, _livraison).pipe(takeUntilDestroyed(this.destroyRef));
  }
}
