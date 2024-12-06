import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Categorie } from '@model/Categorie';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Pagination } from '@model/Pagination';
import { Commande } from '@model/Commande';

export class CommandeService 
{
  private readonly BASE_API = `${environment.urlApi}/commande`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Lister(_dateDebut: Date, _dateFin: Date, _status = 4): Observable<Commande[]>
  {
    let dateDebut = _dateDebut.toISOString().split('T')[0];
    let dateFin = _dateFin.toISOString().split('T')[0];

    const INFOS = { dateDebut, dateFin, status: _status };

    return this.http.get<any[]>(`${this.BASE_API}/lister`, { params: INFOS }).pipe(
        takeUntilDestroyed(this.destroyRef),
        map(listeCommande => 
        {
            for (let i = 0; i < listeCommande.length; i++) 
            {
                let element = listeCommande[i];
                
                element.date = new Date(element.date)
            }

            return listeCommande;
        }) 
    );
  }

  Ajouter(_nom: string): Observable<void>
  {
    const INFOS = { nom: _nom };
    return this.http.post<void>(`${this.BASE_API}/ajouter`, INFOS).pipe(takeUntilDestroyed(this.destroyRef));
  }
}