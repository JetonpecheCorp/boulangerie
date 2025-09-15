import { BooleanInput } from '@angular/cdk/coercion';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDrawerMode, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { ModalParametreComponent } from '@modal/modal-parametre/modal-parametre.component';
import { ERole } from '@enum/ERole';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';

@Component({
    selector: 'app-root',
    imports: [MatTooltipModule, MatListModule, RouterLink, RouterLinkActive, RouterOutlet, MatSidenavModule, MatToolbarModule, MatButtonModule, MatIconModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit
{
  matDialog = inject(MatDialog);
  router = inject(Router);

  mdcBackdrop: BooleanInput = false;
  drawerMode: MatDrawerMode = "push";

  private location = inject(Location);
  private toastrServ = inject(ToastrService);

  constructor(private breakpointObserver: BreakpointObserver) 
  {
    let breakpoint$ = this.breakpointObserver
      .observe([ '(max-width: 500px)']);

    breakpoint$.subscribe(() =>
      this.BreakpointChanges()
    );
  }

  ngOnInit(): void 
  {
    // reconnexion automatique    
    if(sessionStorage.getItem("utilisateur"))
    {
      environment.utilisateur = JSON.parse(sessionStorage.getItem("utilisateur")!);

      const EXP = +JSON.parse(atob(environment.utilisateur.jwt.split(".")[1]))["exp"];            

      // JWT expiré
      if(new Date(EXP * 1_000).getTime() < new Date().getTime())
      {
        sessionStorage.clear();
        environment.utilisateur = null;
        return;
      }

      this.toastrServ.clear();

      // la page précédente est l'application
      if(document.referrer && environment.urlFront.some(x => document.referrer.includes(x)))
        this.location.back();
      
      // je viens d'autre part
      else
      {
        if(environment.utilisateur.role == ERole.Admin)
          this.router.navigateByUrl("/planning");

        else if(environment.utilisateur.role == ERole.Client)
          this.router.navigateByUrl("/commande");
      }
    }
  }

  protected EstConnecter(): boolean
  {
    return environment.utilisateur != undefined && environment.utilisateur != null;
  }

  protected EstAdmin(): boolean
  {
    return environment.utilisateur  && environment.utilisateur.role == ERole.Admin;
  }

  protected Deconnexion(): void
  {
    sessionStorage.clear();
    environment.utilisateur = null;
    this.router.navigateByUrl("/");
  }

  protected OuvrirModalParametre(): void
  {
    this.matDialog.open(ModalParametreComponent);
  }

  private BreakpointChanges(): void 
  {
    if (this.breakpointObserver.isMatched('(max-width: 500px)')) 
    {
      this.drawerMode = "over";
      this.mdcBackdrop = true;
    } 
    else 
    {
      this.drawerMode = "push";
      this.mdcBackdrop = false;
    }
  }
}
