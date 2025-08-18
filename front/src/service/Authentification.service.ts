import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export class AuthentificationService 
{
  private readonly BASE_API = `${environment.urlApi}/authentification`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Connexion(_login: String, _mdp: String): Observable<any>
  {
    const INFOS = { login: _login, mdp: _mdp };

    return this.http.post<any>(`${this.BASE_API}/connexion`, INFOS).pipe(takeUntilDestroyed(this.destroyRef));
  }

  DemandeResetMdp(_mail: string): Observable<void>
  {
    return this.http.get<void>(`${this.BASE_API}/demande-reset-mdp/${_mail}`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  ResetMdp(_mdp: string, _jwtMdpOublier: string): Observable<void>
  {
    const INFOS = { mdp: _mdp };

    let httpHeader = new HttpHeaders();
    httpHeader = httpHeader.append("Authorization", `Bearer ${_jwtMdpOublier}`);

    return this.http.post<void>(`${this.BASE_API}/reset-mdp`, INFOS, {
      headers: httpHeader
    });
  }
}