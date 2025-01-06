import { Component, inject, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { InputComponent } from "../../components/input/input.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ButtonComponent } from "../../components/button/button.component";
import { VehiculeService } from '@service/Vehicule.service';
import { Vehicule } from '@model/Vehicule';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-ajouter-modifier-vehicule',
  standalone: true,
  imports: [MatButtonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, InputComponent, ButtonComponent],
  templateUrl: './ajouter-modifier-vehicule.component.html',
  styleUrl: './ajouter-modifier-vehicule.component.scss'
})
export class AjouterModifierVehiculeComponent implements OnInit
{
  protected form: FormGroup;

  protected btnClicker = signal(false);
  protected labelBtn = signal("Ajouter");

  private vehiculeServ = inject(VehiculeService);
  private dialogRef = inject(MatDialogRef<AjouterModifierVehiculeComponent>);
  private vehicule?: Vehicule = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {
    this.labelBtn.set(this.vehicule ? "Modifier" : "Ajouter");

    this.form = new FormGroup({
      immatriculation: new FormControl(this.vehicule?.immatriculation ?? "", [Validators.max(15)]),
      nom: new FormControl(this.vehicule?.nom ?? "", [Validators.max(100)]),
      infoComplementaire: new FormControl<string | null>(this.vehicule?.infoComplementaire ?? null, [Validators.max(1_000)])
    });
  }

  protected Valider(): void
  {
    if(this.form.invalid || this.btnClicker())
      return;

    this.btnClicker.set(true);

    if(this.vehicule)
    {
      this.vehiculeServ.Modifier(this.vehicule!.idPublic, this.form.value).subscribe({
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
      this.vehiculeServ.Ajouter(this.form.value).subscribe({
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
