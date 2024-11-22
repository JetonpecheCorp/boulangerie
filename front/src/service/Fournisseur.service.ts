import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';
import { Fournisseur } from '@model/Fournisseur';
import { FournisseurExport } from '@model/exports/FournisseurExport';

export class FournissseurService 
{
  private readonly BASE_API = `${environment.urlApi}/fournisseur`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Fournisseur>>
  {
    return this.http.get<Pagination<Fournisseur>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_fournisseur: FournisseurExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _fournisseur).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_idPublicFournisseur: string, _fournisseur: FournisseurExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier/${_idPublicFournisseur}`, _fournisseur).pipe(takeUntilDestroyed(this.destroyRef));
  }
}