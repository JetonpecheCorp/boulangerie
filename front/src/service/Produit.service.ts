import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Pagination } from '@model/Pagination';
import { PaginationExport } from '@model/exports/PaginationExport';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Produit, ProduitLeger } from '@model/Produit';
import { ProduitExport } from '@model/exports/ProduitExport';

export class ProduitService 
{
  private readonly BASE_API = `${environment.urlApi}/produit`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Produit>>
  {
    return this.http.get<Pagination<Produit>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ListerLeger(_pagination: PaginationExport): Observable<Pagination<ProduitLeger>>
  {
    return this.http.get<Pagination<ProduitLeger>>(`${this.BASE_API}/listerLeger`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_produit: ProduitExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _produit).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_produit: ProduitExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier`, _produit).pipe(takeUntilDestroyed(this.destroyRef));
  }
}