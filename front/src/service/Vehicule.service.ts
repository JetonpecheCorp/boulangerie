import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Vehicule } from '@model/Vehicule';
import { VehiculeExport } from '@model/exports/VehiculeExport';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';

export class VehiculeService 
{
  private readonly BASE_API = `${environment.urlApi}/vehicule`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_pagination: PaginationExport): Observable<Pagination<Vehicule>>
  {
    return this.http.get<Pagination<Vehicule>>(`${this.BASE_API}/lister`, { params: _pagination }).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_vehicule: VehiculeExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _vehicule).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_idPublicVehicule: string, _vehicule: VehiculeExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier/${_idPublicVehicule}`, _vehicule).pipe(takeUntilDestroyed(this.destroyRef));
  }
}