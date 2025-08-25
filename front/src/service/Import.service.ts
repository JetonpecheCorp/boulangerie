import { HttpClient } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ETypeRessourceImport } from '@enum/ETypeRessourceImport';
import { Observable } from 'rxjs';
import { ErreurValidationCSV } from '@model/ErreurValidationCSV';

export class ImportService 
{
  private readonly BASE_API = `${environment.urlApi}/import`;

  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Importer(_ressource: ETypeRessourceImport, _fichierCSV: File): Observable<ErreurValidationCSV[]>
  {
    const FORM_DATA = new FormData();
    FORM_DATA.append("fichier", _fichierCSV, _fichierCSV.name);

    return this.http.post<ErreurValidationCSV[]>(`${this.BASE_API}/${_ressource}`, FORM_DATA).pipe(takeUntilDestroyed(this.destroyRef));
  }
}