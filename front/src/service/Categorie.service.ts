import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Categorie } from '@model/Categorie';

export class CategorieService 
{
  private readonly BASE_API = `${environment.urlApi}/categorie`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(): Observable<Categorie[]>
  {
    return this.http.get<Categorie[]>(`${this.BASE_API}/lister`).pipe(takeUntilDestroyed(this.destroyRef));
  }
}