import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Pagination } from '@model/Pagination';
import { PaginationExport } from '@model/exports/PaginationExport';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Client, ClientLeger } from '@model/Client';

export class ClientService 
{
  private readonly BASE_API = `${environment.urlApi}/client`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Client>>
  {
    return this.http.get<Pagination<Client>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ListerLeger(_pagination: PaginationExport): Observable<Pagination<ClientLeger>>
  {
    return this.http.get<Pagination<ClientLeger>>(`${this.BASE_API}/listerLeger`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }
}