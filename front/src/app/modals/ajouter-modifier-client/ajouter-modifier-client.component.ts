import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { InputComponent } from "@component/input/input.component";
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Client } from '@model/Client';
import { ButtonComponent } from "@component/button/button.component";
import { TextareaComponent } from "@component/textarea/textarea.component";
import { ClientService } from '@service/Client.service';

@Component({
  selector: 'app-ajouter-modifier-client',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, InputComponent, ButtonComponent, TextareaComponent],
  templateUrl: './ajouter-modifier-client.component.html',
  styleUrl: './ajouter-modifier-client.component.scss'
})
export class AjouterModifierClientComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);
  protected labelBtn = signal("");

  private dialogRef = inject(MatDialogRef<AjouterModifierClientComponent>);
  private matDialogData?: Client = inject(MAT_DIALOG_DATA);
  private clientServ = inject(ClientService);

  ngOnInit(): void 
  {
    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");

    this.form = new FormGroup({
      nom: new FormControl(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(300)]),
      mail: new FormControl(this.matDialogData?.mail, [Validators.email]),
      telephone: new FormControl(this.matDialogData?.mail, [Validators.maxLength(20)]),
      adresse: new FormControl(this.matDialogData?.adresse ?? "", [Validators.required, Validators.maxLength(800)]),
      adresseFacturation: new FormControl(this.matDialogData?.adresseFacturation, [Validators.maxLength(800)]),
      infoComplementaire: new FormControl(this.matDialogData?.infoComplementaire, [Validators.maxLength(1_000)])
    });
  }

  protected Valider(): void
  {    
    if(this.form.invalid || this.btnClicker())
      return;
    
    this.btnClicker.set(true);
    
    if(this.matDialogData)
    {
      this.clientServ.Modifier(this.matDialogData.idPublic, this.form.value).subscribe({
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
      this.clientServ.Ajouter(this.form.value).subscribe({
        next: (idPublic) =>
        {
          this.btnClicker.set(false);
          this.form.value.idPublic = idPublic;
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }
}
