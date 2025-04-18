import { Component, inject, OnInit, signal } from '@angular/core';
import { InputComponent } from "@component/input/input.component";
import { ButtonComponent } from "@component/button/button.component";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Groupe } from '@model/Groupe';
import { GroupeService } from '@service/Groupe.service';

@Component({
  selector: 'app-ajouter-modifier-groupe',
  standalone: true,
  imports: [MatIconModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, ReactiveFormsModule, InputComponent, ButtonComponent],
  templateUrl: './ajouter-modifier-groupe.component.html',
  styleUrl: './ajouter-modifier-groupe.component.scss'
})
export class AjouterModifierGroupeComponent implements OnInit
{
  protected form: FormGroup;
  protected labelBtn = signal<string>("Ajouter");
  protected btnClicker = signal(false);

  private matDialogData?: Groupe = inject(MAT_DIALOG_DATA);
  private groupeServ = inject(GroupeService);
  private dialogRef = inject(MatDialogRef<AjouterModifierGroupeComponent>);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      nom: new FormControl(this.matDialogData?.nom, [Validators.required, Validators.maxLength(300)]),
      adresse: new FormControl(this.matDialogData?.adresse, [Validators.required, Validators.maxLength(800)])
    });

    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");
  }

  protected Valider(): void
  {    
    if(this.form.invalid ||this.btnClicker())
      return;

    if(this.matDialogData)
    {
      this.groupeServ.Modifier(this.matDialogData.id, this.form.value).subscribe({
        next: () =>
          {
            this.btnClicker.set(false);
            this.dialogRef.close(this.form.value);
          },
          error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      this.groupeServ.Ajouter(this.form.value).subscribe({
        next: (retour) =>
        {
          let groupe: Groupe = 
          {
            adresse: this.form.value.adresse,
            nom: this.form.value.nom,
            connexionBloquer: false,
            id: retour
          };
  
          this.dialogRef.close(groupe);
        }
      });
    }
  }
}
