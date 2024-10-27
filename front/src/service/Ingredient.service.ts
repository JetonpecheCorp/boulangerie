import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

export class IngredientService 
{
  private readonly BASE_API = `${environment.urlApi}/ingredient`;

  private http: HttpClient = inject(HttpClient);

  Lister()
  {
    const INFOS = { numPage: 1, nbParPage: 10 };
    return this.http.get(`${this.BASE_API}/lister`, { params: INFOS });
  }
}