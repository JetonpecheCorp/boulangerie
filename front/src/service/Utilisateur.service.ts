import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Utilisateur, UtilisateurLeger } from '@model/Utilisateur';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';

export class UtilisateurService 
{
  private readonly BASE_API = `${environment.urlApi}/utilisateur`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Utilisateur>>
  {
    return this.http.get<Pagination<Utilisateur>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ListerLeger(_pagination: PaginationExport): Observable<Pagination<UtilisateurLeger>>
  {
    return this.http.get<Pagination<UtilisateurLeger>>(`${this.BASE_API}/listerLeger`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }
}