import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

export class AuthentificationService 
{
  private readonly BASE_API = `${environment.urlApi}/authentification`;

  private http: HttpClient = inject(HttpClient);

  Connexion(_login: String, _mdp: String): Observable<any>
  {
    const INFOS = { login: _login, mdp: _mdp };

    return this.http.post<any>(`${this.BASE_API}/connexion`, INFOS);
  }
}