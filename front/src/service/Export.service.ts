import { HttpClient, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { DestroyRef, inject } from '@angular/core';
import { environment } from '../environments/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EStatusCommande } from '@enum/EStatusCommande';
import { ToastrService } from 'ngx-toastr';

export class ExportService 
{
  private readonly BASE_API = `${environment.urlApi}/export`;

  private toastrServ: ToastrService = inject(ToastrService);
  private http: HttpClient = inject(HttpClient);
  private destroyRef: DestroyRef = inject(DestroyRef);

  Client(): void
  {
    this.http.get(`${this.BASE_API}/client`,
        {observe: "response", responseType: "blob" }
      )
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(reponse => this.Telecharger(reponse));
  }

  Utilisateur(): void
  {
    this.http.get(`${this.BASE_API}/utilisateur`,
        {observe: "response", responseType: "blob" }
      )
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(reponse => this.Telecharger(reponse));
  }
  
  Produit(): void
  {
    this.http.get(`${this.BASE_API}/produit`,
        {observe: "response", responseType: "blob" }
      )
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(reponse => this.Telecharger(reponse));
  }

  Commande(_dateDebut: Date, _dateFin: Date, _status: EStatusCommande): void
  {    
    const INFO = { 
        dateDebut: _dateDebut.toISOFormat(), 
        dateFin: _dateFin.toISOFormat(), 
        status: _status 
    };

    this.http.get(`${this.BASE_API}/commande`,
      {observe: "response", responseType: "blob", params: INFO }
    )
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(reponse => this.Telecharger(reponse));
  }

  private Telecharger(_reponse: HttpResponse<Blob>): void
  {
    if(_reponse.status == HttpStatusCode.NoContent)
    {
      this.toastrServ.info("Il n'y a aucune information à télécharger");
      return;
    }
    
    const NOM_FICHIER = _reponse.headers.get("content-disposition")!
        .replace("filename=", "")
        .split(";")[1];
    
    let a = document.createElement("a");
    a.download = NOM_FICHIER;
    let url = URL.createObjectURL(_reponse.body as Blob);
    a.href = url;
    a.click();

    URL.revokeObjectURL(url);
  }
}