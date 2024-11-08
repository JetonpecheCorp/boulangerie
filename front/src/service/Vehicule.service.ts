import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Tva } from '@model/Tva';
import { Vehicule } from '@model/Vehicule';
import { VehiculeExport } from '@model/exports/VehiculeExport';

export class TvaService 
{
  private readonly BASE_API = `${environment.urlApi}/vehicule`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(): Observable<Vehicule[]>
  {
    return this.http.get<Vehicule[]>(`${this.BASE_API}/lister`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_vehicule: VehiculeExport): Observable<void>
  {
    return this.http.post<void>(`${this.BASE_API}/ajouter`, _vehicule).pipe(takeUntilDestroyed(this.destroyRef));
  }
}