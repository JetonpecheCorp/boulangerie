import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { DialogPosition, MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CalendrierJourComponent } from "../../pages/test/calendrier-jour/calendrier-jour.component";
import { ButtonComponent } from "../../components/button/button.component";
import { ModalAjouterCommmandeComponent } from '@modal/modal-ajouter-commmande/modal-ajouter-commmande.component';

@Component({
  selector: 'app-modal-calendrier-jour',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, CalendrierJourComponent, ButtonComponent],
  templateUrl: './modal-calendrier-jour.component.html',
  styleUrl: './modal-calendrier-jour.component.scss'
})
export class ModalCalendrierJourComponent 
{
  protected info = inject(MAT_DIALOG_DATA);
  private matDialog = inject(MatDialog);
  
  protected OuvrirModalAjouterCommande(): void
  {
    this.matDialog.open(ModalAjouterCommmandeComponent, { 
      width: "700px"
    });
  }
}
