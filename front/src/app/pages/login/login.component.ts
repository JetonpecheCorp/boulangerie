import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthentificationService } from '@service/Authentification.service'
import { ThemeService } from '@service/ThemeService.Service';
import { InputComponent } from "@component/input/input.component";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from '../../../environments/environment';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ButtonComponent } from "@component/button/button.component";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, MatProgressSpinnerModule, MatCardModule, ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, InputComponent, ButtonComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);

  private authentificationServ = inject(AuthentificationService);
  private serv = inject(ThemeService);
  private destroyRef = inject(DestroyRef);

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

    this.authentificationServ.Connexion(FORM.login, FORM.mdp).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
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
