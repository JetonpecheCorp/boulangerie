import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Recette } from '@model/Recette';

export class RecetteService 
{
  private readonly BASE_API = `${environment.urlApi}/recette`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_idPublicProduit: string): Observable<Recette[]>
  { 
    return this.http.get<Recette[]>(`${this.BASE_API}/lister/${_idPublicProduit}`).pipe(takeUntilDestroyed(this.destroyRef));
  }
}