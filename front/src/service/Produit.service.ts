import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Pagination } from '@model/Pagination';
import { PaginationExport } from '@model/exports/PaginationExport';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export class ProduitService 
{
  private readonly BASE_API = `${environment.urlApi}/produit`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<any>>
  {
    return this.http.get<Pagination<any>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }
}