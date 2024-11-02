import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Tva } from '@model/Tva';

export class TvaService 
{
  private readonly BASE_API = `${environment.urlApi}/tva`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(): Observable<Tva[]>
  {
    return this.http.get<Tva[]>(`${this.BASE_API}/lister`).pipe(takeUntilDestroyed(this.destroyRef));
  }
}