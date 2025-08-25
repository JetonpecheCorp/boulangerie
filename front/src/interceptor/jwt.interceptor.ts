
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Router } from '@angular/router';

export const JwtInterceptor: HttpInterceptorFn = (req, next) => 
{
  let toastrServ = inject(ToastrService);
  let router = inject(Router);

  console.log(req);

  if(environment.utilisateur && !req.url.includes("reset-mdp"))
  {
    req = req.clone({
      headers: req.headers.set("Authorization", `Bearer ${environment.utilisateur.jwt}`)
    });
  }

  return next(req).pipe(
    catchError(
      (erreur) =>
      {
        console.log(erreur);

        switch (erreur.status) 
        {
          case 500:
            toastrServ.warning("Erreur interne c'est produite");
            break;

          case 401:
            toastrServ.error("Veuillez-vous connecter");
            localStorage.removeItem("utilisateur");
            router.navigateByUrl("/");
            break;

          case 403:
            toastrServ.error("Vous n'avez pas l'autorisation");
            break;

          case 404:
          case 400:
            toastrServ.error(erreur.error);
            break;

          case 429:
            toastrServ.error("Spam détecté veuillez patienter");
            break;
        
          default:
            toastrServ.error("Erreur pas de réseau");
            break;
        }
          
        return throwError(() => null);
      }
    )
  );
};