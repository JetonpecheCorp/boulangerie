import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { InputComponent } from "../../components/input/input.component";
import { IngredientService } from '@service/Ingredient.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Ingredient } from '@model/Ingredient';

@Component({
  selector: 'app-ajouter-modifier-ingredient',
  standalone: true,
  imports: [MatDialogModule, MatProgressSpinnerModule, MatButtonModule, MatFormFieldModule, ReactiveFormsModule, InputComponent],
  templateUrl: './ajouter-modifier-ingredient.component.html',
  styleUrl: './ajouter-modifier-ingredient.component.scss'
})
export class AjouterModifierIngredientComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);

  private ingredientServ = inject(IngredientService);
  private dialogRef = inject(MatDialogRef<AjouterModifierIngredientComponent>);
  private matDialogData?: Ingredient = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      idPublic: new FormControl(this.matDialogData?.idPublic),
      nom: new FormControl<string>(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(200)]),
      codeInterne: new FormControl<string | null>(this.matDialogData?.codeInterne ?? null, [Validators.maxLength(100)]),
      stock: new FormControl<number>(this.matDialogData?.stock ?? 0),
      stockAlert: new FormControl<number>(this.matDialogData?.stockAlert ?? 0)
    });
  }

  protected Valider(): void
  {    
    if(this.form.invalid || this.btnClicker())
      return;
    
    this.btnClicker.set(true);
    
    if(this.matDialogData)
    {
      this.ingredientServ.Modifier(this.form.value).subscribe({
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
      delete this.form.value.idPublic;

      this.ingredientServ.Ajouter(this.form.value).subscribe({
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
