import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { LivraisonComponent } from "../../pages/livraison/livraison.component";

@Component({
  selector: 'app-modal-livraison',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, LivraisonComponent],
  templateUrl: './modal-livraison.component.html',
  styleUrl: './modal-livraison.component.scss'
})
export class ModalLivraisonComponent 
{
  protected dialogData = inject(MAT_DIALOG_DATA);
}
