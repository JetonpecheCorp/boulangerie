import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Categorie } from '@model/Categorie';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';

export class CategorieService 
{
  private readonly BASE_API = `${environment.urlApi}/categorie`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(): Observable<Categorie[]>
  {
    return this.http.get<Categorie[]>(`${this.BASE_API}/lister`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ListerPaginer(_pagination: PaginationExport): Observable<Pagination<Categorie>>
  {
    return this.http.get<Pagination<Categorie>>(`${this.BASE_API}/listerPaginer`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_nom: string): Observable<void>
  {
    const INFOS = { nom: _nom };
    return this.http.post<void>(`${this.BASE_API}/ajouter`, INFOS).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_idPublic: string, _nom: string): Observable<void>
  {
    const INFOS = { nom: _nom };
    return this.http.put<void>(`${this.BASE_API}/modifier/${_idPublic}`, INFOS).pipe(takeUntilDestroyed(this.destroyRef));
  }
}