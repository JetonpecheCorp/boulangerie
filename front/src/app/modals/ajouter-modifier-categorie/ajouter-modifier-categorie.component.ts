import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { InputComponent } from "@component/input/input.component";
import { Ingredient } from '@model/Ingredient';
import { CategorieService } from '@service/Categorie.service';
import { ButtonComponent } from '@component/button/button.component';

@Component({
  selector: 'app-ajouter-modifier-categorie',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatFormFieldModule, ReactiveFormsModule, InputComponent, ButtonComponent],
  templateUrl: './ajouter-modifier-categorie.component.html',
  styleUrl: './ajouter-modifier-categorie.component.scss'
})
export class AjouterModifierCategorieComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);
  protected labelBtn = signal("");

  private categorieServ = inject(CategorieService);
  private dialogRef = inject(MatDialogRef<AjouterModifierCategorieComponent>);
  private matDialogData?: Ingredient = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {
    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");

    this.form = new FormGroup({
      idPublic: new FormControl(this.matDialogData?.idPublic),
      nom: new FormControl<string>(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(300)])
    });
  }

  protected Valider(): void
  {    
    if(this.form.invalid || this.btnClicker())
      return;
    
    this.btnClicker.set(true);
    
    if(this.matDialogData)
    {
      const FORM = this.form.value;
      this.categorieServ.Modifier(FORM.idPublic, FORM.nom).subscribe({
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
      this.categorieServ.Ajouter(this.form.value.nom).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }
}
