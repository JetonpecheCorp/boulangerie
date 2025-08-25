import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';
import { GroupeExport } from '@model/exports/GroupeExport';
import { Groupe } from '@model/Groupe';

export class GroupeService 
{
  private readonly BASE_API = `${environment.urlApi}/groupe`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(): Observable<Groupe[]>
  {
    return this.http.get<Groupe[]>(`${this.BASE_API}/lister`).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Ajouter(_groupeExport: GroupeExport): Observable<number>
  {
    return this.http.post<number>(`${this.BASE_API}/ajouter`, _groupeExport).pipe(takeUntilDestroyed(this.destroyRef));
  }

  Modifier(_idGroupe: number, _groupeExport: GroupeExport): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/modifier/${_idGroupe}`, _groupeExport).pipe(takeUntilDestroyed(this.destroyRef));
  }

  BloquerDebloquer(_idGroupe: number): Observable<void>
  {
    return this.http.put<void>(`${this.BASE_API}/bloquer-debloquer/${_idGroupe}`, null).pipe(takeUntilDestroyed(this.destroyRef));
  }
}