import { Component, DestroyRef, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CalendrierJourComponent } from "../../pages/test/calendrier-jour/calendrier-jour.component";
import { ButtonComponent } from "../../components/button/button.component";
import { ModalAjouterCommmandeComponent } from '@modal/modal-ajouter-commmande/modal-ajouter-commmande.component';
import { Commande } from '@model/Commande';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

type Info =
{
  date: Date,
  listeCommande: Commande[]
}

@Component({
  selector: 'app-modal-calendrier-jour',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, CalendrierJourComponent, ButtonComponent],
  templateUrl: './modal-calendrier-jour.component.html',
  styleUrl: './modal-calendrier-jour.component.scss'
})
export class ModalCalendrierJourComponent 
{
  protected info: Info = inject(MAT_DIALOG_DATA);
  private matDialog = inject(MatDialog);
  private listeCommandeAjouter = signal<Commande[]>([]);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(MatDialogRef<ModalCalendrierJourComponent>);
  
  protected OuvrirModalAjouterCommande(): void
  {
    const DIALOG_REF = this.matDialog.open(ModalAjouterCommmandeComponent, { 
      width: "700px",
      data: {
        date: this.info.date
      }
    });

    DIALOG_REF.afterClosed().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (retour?: Commande) =>
      {
        if(!retour) 
          return;

        this.listeCommandeAjouter.update(x => [...x, retour]);
        this.info.listeCommande.push(retour);
      }
    });
  }

  protected OuvrirModalModifierCommande(_commande: Commande): void
  {
    const DIALOG_REF = this.matDialog.open(ModalAjouterCommmandeComponent, { 
      data: {
        date: this.info.date
      }
    });

    DIALOG_REF.afterClosed().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (retour: Commande) =>
      {
        const INDEX = this.info.listeCommande.findIndex(x => x.numero == _commande.numero);
        this.info.listeCommande[INDEX] = retour;
      }
    });
  }

  protected Fermer(): void
  {
    this.dialogRef.close(this.listeCommandeAjouter());
  }
}
