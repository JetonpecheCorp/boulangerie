import { ChangeDetectionStrategy, Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CalendrierJourComponent } from "../../pages/test/calendrier-jour/calendrier-jour.component";
import { ButtonComponent } from "../../components/button/button.component";
import { ModalAjouterCommmandeComponent } from '@modal/modal-ajouter-commmande/modal-ajouter-commmande.component';
import { Commande } from '@model/Commande';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ConvertionEnum, EStatusCommande } from '@enum/EStatusCommande';
import { MatSelectModule } from '@angular/material/select';

type Info =
{
  date: Date,
  listeCommande: Commande[]
}

@Component({
  selector: 'app-modal-calendrier-jour',
  standalone: true,
  imports: [MatSelectModule, MatDialogModule, MatButtonModule, CalendrierJourComponent, ButtonComponent],
  templateUrl: './modal-calendrier-jour.component.html',
  styleUrl: './modal-calendrier-jour.component.scss',
})
export class ModalCalendrierJourComponent implements OnInit
{
  protected eStatusCommande = EStatusCommande;
  protected info: Info = inject(MAT_DIALOG_DATA);
  protected listeCommandeFiltrer = signal<Commande[]>([]);

  private matDialog = inject(MatDialog);
  private listeCommandeAjouter = signal<Commande[]>([]);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(MatDialogRef<ModalCalendrierJourComponent>);
  private status = signal<EStatusCommande>(EStatusCommande.Tout);

  ngOnInit(): void 
  {
    this.listeCommandeFiltrer.set(this.info.listeCommande);  
  }

  protected ConvertionEnumCommande(_status: EStatusCommande): string
  {
    return ConvertionEnum.StatusCommande(_status);
  }

  protected FiltrerParStatus(_status: EStatusCommande): void
  {
    const LISTE = this.info.listeCommande.filter(x => x.status == _status);
    this.listeCommandeFiltrer.set(LISTE);
  }

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

        this.FiltrerParStatus(this.status());
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
      next: (retour?: Commande) =>
      {
        if(!retour)
          return;

        const INDEX = this.info.listeCommande.findIndex(x => x.numero == _commande.numero);
        this.info.listeCommande[INDEX] = retour;

        this.FiltrerParStatus(this.status());
      }
    });
  }

  protected Fermer(): void
  {
    this.dialogRef.close(this.listeCommandeAjouter());
  }
}
