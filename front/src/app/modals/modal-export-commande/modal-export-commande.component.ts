import { Component, inject, OnInit, signal } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { ButtonComponent } from "../../components/button/button.component";
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { EStatusCommande } from '@enum/EStatusCommande';
import { ExportService } from '@service/Export.service';

type DateInterval = {
  dateDebut: Date,
  dateFin: Date
}

@Component({
  selector: 'app-modal-export-commande',
  standalone: true,
  imports: [MatSelectModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule, ButtonComponent, MatNativeDateModule, MatDatepickerModule, ReactiveFormsModule],
  templateUrl: './modal-export-commande.component.html',
  styleUrl: './modal-export-commande.component.scss'
})
export class ModalExportCommandeComponent implements OnInit
{
  protected form: FormGroup;
  protected dialogData: DateInterval = inject(MAT_DIALOG_DATA);
  protected btnClicker = signal<boolean>(false);
  
  private exportServ = inject(ExportService);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      dateDebut: new FormControl<Date | undefined>(this.dialogData.dateDebut, [Validators.required]),
      dateFin: new FormControl<Date | undefined>(this.dialogData.dateFin, [Validators.required]),
      status: new FormControl<EStatusCommande>(EStatusCommande.Tout, [Validators.required])
    });
  }

  protected Exporter(): void
  {
    if(this.btnClicker())
      return;
    
    const FORM = this.form.value;

    this.btnClicker.set(true);

    this.exportServ.Commande(FORM.dateDebut, FORM.dateFin, FORM.status);
    
    this.btnClicker.set(false);
  }
}
