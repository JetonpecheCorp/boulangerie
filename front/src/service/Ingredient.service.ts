import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { Pagination } from '../models/Pagination';
import { Ingredient } from '../models/Ingredient';

export class IngredientService 
{
  private readonly BASE_API = `${environment.urlApi}/ingredient`;

  private http: HttpClient = inject(HttpClient);

  Lister(_numPage: number, _nbParPage: number): Observable<Pagination<Ingredient>>
  {
    const INFOS = { numPage: _numPage, nbParPage: _nbParPage };
    return this.http.get<Pagination<Ingredient>>(`${this.BASE_API}/lister`, { params: INFOS });
  }
}