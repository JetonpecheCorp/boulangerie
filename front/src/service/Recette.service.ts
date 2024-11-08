import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Recette } from '@model/Recette';
import { RecetteExport } from '@model/exports/RecetteExport';

export class RecetteService 
{
  private readonly BASE_API = `${environment.urlApi}/recette`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_idPublicProduit: string): Observable<Recette[]>
  { 
    return this.http.get<Recette[]>(`${this.BASE_API}/lister/${_idPublicProduit}`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_recette: RecetteExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _recette).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ModifierQuantite(_recette: RecetteExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifierQuantite`, _recette).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Supprimer(_idPublicProduit: string, _idPublicIngredient: string): Observable<void>
  {
    const INFOS = {
      idPublicIngredient: _idPublicIngredient,
      idPublicProduit: _idPublicProduit
    };

    return this.http.delete<void>(`${this.BASE_API}/supprimer`, { body: INFOS }).pipe(takeUntilDestroyed(this.destroyRef));
  }
}