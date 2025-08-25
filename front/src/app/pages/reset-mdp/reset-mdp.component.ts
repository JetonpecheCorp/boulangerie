import { Component, inject, OnInit, signal } from '@angular/core';
import { InputComponent } from "@component/input/input.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from "@component/button/button.component";
import { MatCardModule } from '@angular/material/card';
import { AuthentificationService } from '@service/Authentification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-reset-mdp',
    imports: [InputComponent, ReactiveFormsModule, ButtonComponent, MatCardModule],
    templateUrl: './reset-mdp.component.html',
    styleUrl: './reset-mdp.component.scss'
})
export class ResetMdpComponent implements OnInit
{
  form: FormGroup;
  btnClicker = signal(false);

  jwt = signal<string | null>(null);

  private authServ = inject(AuthentificationService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    this.jwt.set(this.route.snapshot.paramMap.get('p'));

    if(this.jwt())
    {
      this.form = new FormGroup({
        mdp: new FormControl("", [Validators.required]),
        mdpConfirmer: new FormControl("", [Validators.required])
      });
    }
    else
    {
      this.form = new FormGroup({
        mail: new FormControl("", [Validators.email])
      });
    }
  }

  protected Valider(): void
  {
    if(this.form.invalid || this.btnClicker())
      return;

    this.btnClicker.set(true);

    if(this.jwt())
    {
      this.authServ.ResetMdp(this.form.value.mdp, this.jwt()!).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.toastrServ.success("Le mot de passe a été modifié");
          this.router.navigateByUrl("/");
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      this.authServ.DemandeResetMdp(this.form.value.mail).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.toastrServ.success("Un mail a été envoyé");
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }
}
