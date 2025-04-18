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
    this.EnvoyerRequete("client");
  }

  Utilisateur(): void
  {
    this.EnvoyerRequete("utilisateur");
  }
  
  Produit(): void
  {
    this.EnvoyerRequete("produit");
  }

  Fournisseur(): void
  {
    this.EnvoyerRequete("fournisseur");
  }

  Commande(_dateDebut: Date, _dateFin: Date, _status: EStatusCommande): void
  {    
    const INFO = { 
        dateDebut: _dateDebut.toISOFormat(), 
        dateFin: _dateFin.toISOFormat(), 
        status: _status 
    };

    this.EnvoyerRequete("commande", INFO);
  }

  private EnvoyerRequete(_route: string, _params?: any): void
  {
    this.http.get(`${this.BASE_API}/${_route}`,
      {observe: "response", responseType: "blob", params: _params }
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