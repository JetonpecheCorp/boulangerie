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
import { ERole } from '@enum/ERole';

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
  private router = inject(Router);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      login: new FormControl<string>("", [Validators.required]),
      mdp: new FormControl<string>("", [Validators.required])
    }); 
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

        if(retour.role == ERole.Admin)
          this.router.navigateByUrl("/planning");

        else if(retour.role == ERole.Client)
          this.router.navigateByUrl("/commande");

        sessionStorage.setItem("utilisateur", JSON.stringify(environment.utilisateur));
      },
      error: () => this.btnClicker.set(false)
    });
  }
}
