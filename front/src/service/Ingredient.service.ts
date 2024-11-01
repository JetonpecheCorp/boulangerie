import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Pagination } from '../models/Pagination';
import { Ingredient } from '../models/Ingredient';
import { PaginationExport } from '../models/exports/PaginationExport';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IngredientExport } from '@model/exports/IngredientExport';

export class IngredientService 
{
  private readonly BASE_API = `${environment.urlApi}/ingredient`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Ingredient>>
  {
    return this.http.get<Pagination<Ingredient>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_ingredient: IngredientExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _ingredient).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_ingredient: IngredientExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier`, _ingredient).pipe(takeUntilDestroyed(this.destroyRef));
  }
}