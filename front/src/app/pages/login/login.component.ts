import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthentificationService } from '@service/Authentification.service'
import { InputComponent } from "@component/input/input.component";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from '../../../environments/environment';
import { ButtonComponent } from "@component/button/button.component";
import { Router, RouterLink } from '@angular/router';
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-login',
    imports: [RouterLink, MatProgressSpinnerModule, MatCardModule, ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, InputComponent, ButtonComponent],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);

  private authentificationServ = inject(AuthentificationService);
  private location = inject(Location);
  private toastrServ = inject(ToastrService);
  private router = inject(Router);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      login: new FormControl<string>("", [Validators.required]),
      mdp: new FormControl<string>("", [Validators.required])
    }); 
    
    // reconnexion automatique
    setTimeout(() => 
    {
      if(sessionStorage.getItem("utilisateur"))
      {      
        environment.utilisateur = JSON.parse(sessionStorage.getItem("utilisateur")!);

        const EXP = +JSON.parse(atob(environment.utilisateur.jwt.split(".")[1]))["exp"];

        // JWT expiré
        if(new Date(EXP * 1_000).getTime() < new Date().getTime())
        {
          sessionStorage.clear();
          return;
        }
          
        this.toastrServ.clear();
        this.location.back();
      }
    }, 0);
  }

  protected OnConnexion(): void
  {    
    if(this.form.invalid || this.btnClicker())
      return;

    this.btnClicker.set(true);

    const FORM = this.form.value;

    this.authentificationServ.Connexion(FORM.login, FORM.mdp).subscribe({
      next: (retour) =>
      {
        this.btnClicker.set(false);
        
        retour.role = JSON.parse(atob(retour.jwt.split(".")[1]))["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        
        environment.utilisateur = retour;        
        this.router.navigateByUrl("/planning");
        sessionStorage.setItem("utilisateur", JSON.stringify(environment.utilisateur));
      },
      error: () => this.btnClicker.set(false)
    });
  }
}
