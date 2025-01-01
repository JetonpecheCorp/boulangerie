import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CalendrierJourComponent } from "../../pages/test/calendrier-jour/calendrier-jour.component";
import { ButtonComponent } from "../../components/button/button.component";
import { ModalAjouterCommmandeComponent } from '@modal/modal-ajouter-commmande/modal-ajouter-commmande.component';
import { Commande, ProduitCommandeExistant } from '@model/Commande';

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
    let listeProduitExistant: ProduitCommandeExistant[] = [];

    for (const element of this.info.listeCommande as Commande[]) 
    {
      for (const element2 of element.listeProduit) 
      {
        const INDEX = listeProduitExistant.findIndex(x => x.idPublic == element2.idPublic);

        if(INDEX == -1)
        {
          listeProduitExistant.push({
            idPublic: element2.idPublic,
            nom: element2.nom
          });
        }
      }
    }

    this.matDialog.open(ModalAjouterCommmandeComponent, { 
      width: "700px",
      data: {
        date: this.info.date,
        listeProduitExistant: listeProduitExistant
      }
    });
  }
}
