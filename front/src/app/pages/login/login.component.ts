import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthentificationService } from '@service/Authentification.service'
import { ThemeService } from '@service/ThemeService.Service';
import { InputComponent } from "../../components/input/input.component";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatCardModule, ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, InputComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);

  private authentificationServ = inject(AuthentificationService);
  private serv = inject(ThemeService);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      login: new FormControl<String>("", [Validators.required, Validators.maxLength(10), Validators.email]),
      mdp: new FormControl<String>("", [Validators.required])
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
        environment.utilisateur = retour;
        this.btnClicker.set(false);
      },
      error: () => this.btnClicker.set(false)
    });
  }

  generateDynamicTheme(ev: Event)
  {
    this.serv.generateDynamicTheme(ev);
  }
}
