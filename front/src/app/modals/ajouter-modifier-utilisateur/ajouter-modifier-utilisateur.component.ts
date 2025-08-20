import { Component, inject, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Utilisateur } from '@model/Utilisateur';
import { ButtonComponent } from "../../components/button/button.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from "../../components/input/input.component";
import { MatButtonModule } from '@angular/material/button';
import { UtilisateurService } from '@service/Utilisateur.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-ajouter-modifier-utilisateur',
    imports: [MatDialogModule, MatCheckboxModule, MatButtonModule, ReactiveFormsModule, ButtonComponent, InputComponent],
    templateUrl: './ajouter-modifier-utilisateur.component.html',
    styleUrl: './ajouter-modifier-utilisateur.component.scss'
})
export class AjouterModifierUtilisateurComponent implements OnInit
{
  protected labelBtn = signal<string>("Ajouter");
  protected form: FormGroup;
  protected btnClicker = signal<boolean>(false);

  private matDialogData?: Utilisateur = inject(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<AjouterModifierUtilisateurComponent>);
  private utilisateurServ = inject(UtilisateurService);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");

    this.form = new FormGroup({
      nom: new FormControl(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(200)]),
      prenom: new FormControl(this.matDialogData?.prenom ?? "", [Validators.required, Validators.maxLength(200)]),
      mail: new FormControl(this.matDialogData?.mail ?? "", [Validators.required, Validators.email]),
      telephone: new FormControl(this.matDialogData?.telephone ?? "", [Validators.maxLength(20)]),
      estAdmin: new FormControl(this.matDialogData?.estAdmin ?? false, [Validators.required]),
    });

    if(!this.matDialogData)
    {
      this.form.addControl<string>(
        "mdp", new FormControl("", [
          Validators.required, 
          Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*\-_=+;.,§£§]).{8,}$"), 
          Validators.minLength(8)
        ])
      );
    }
  }

  protected Valider(): void
  { 
    if(this.form.invalid || this.btnClicker())
      return;

    this.btnClicker.set(true);

    if(this.matDialogData)
    {
      this.utilisateurServ.Modifier(this.matDialogData.idPublic, this.form.value).subscribe({
        next: () =>
        {
          this.btnClicker.set(true);
          this.toastrServ.success("L'employé(e) a été modifié(e)");
    
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      this.utilisateurServ.Ajouter(this.form.value).subscribe({
        next: (idPublic) =>
        {
          this.btnClicker.set(true);
  
          this.form.value.idPublic = idPublic;
          this.toastrServ.success("L'employé(e) a été ajouté(e");
  
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }
}
