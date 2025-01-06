import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Commande } from '@model/Commande';
import { CommandeService } from '@service/Commande.service';
import { CalendrierSemaineComponent } from "./calendrier-semaine/calendrier-semaine.component";
import { CalendrierJourComponent } from "./calendrier-jour/calendrier-jour.component";
import { CalendrierMoisComponent } from "./calendrier-mois/calendrier-mois.component";
import {MatButtonToggleChange, MatButtonToggleModule} from '@angular/material/button-toggle';
import { MatDialog } from '@angular/material/dialog';
import { ModalCalendrierJourComponent } from '@modal/modal-calendrier-jour/modal-calendrier-jour.component';
import { RetourCalendrierMois } from '@model/calendrier/RetourCalendrier';
import { MatSidenavModule } from '@angular/material/sidenav';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommandeFiltreExport } from '@model/exports/CommandeExport';
import { EStatusCommande, ConvertionEnum } from '@enum/EStatusCommande';
import {MatSelectModule} from '@angular/material/select';

enum EModeCalendrier 
{
  Jour,
  Semaine,
  Mois
}

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [MatSelectModule, MatSidenavModule, MatButtonToggleModule, MatButtonModule, MatIconModule, MatCardModule, CalendrierSemaineComponent, CalendrierJourComponent, CalendrierMoisComponent],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss'
})
export class TestComponent implements OnInit
{
  dateJour = signal(new Date());
  listeCommande = signal<Commande[]>([]);

  protected mode = signal(EModeCalendrier.Mois);
  protected eModeCalendrier = EModeCalendrier;
  protected eStatusCommande = EStatusCommande;

  private commandeServ = inject(CommandeService);
  private matDialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);

  ngOnInit(): void 
  {
    this.ListerCommande();
  }

  ChangerAffichage(_event: MatButtonToggleChange): void
  {
    this.mode.set(_event.value);
  }

  protected FiltrerParStatus(_status: EStatusCommande)
  {
    this.ListerCommande(_status);
  }

  protected ConvertionEnumCommande(_status: EStatusCommande): string
  {
    return ConvertionEnum.StatusCommande(_status);
  }

  protected OuvrirModalCalendrierJour(_info: RetourCalendrierMois)
  {    
    const DIALOG_REF = this.matDialog.open(ModalCalendrierJourComponent, { 
      data: _info,
      width: "80%"
    });

    DIALOG_REF.afterClosed().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (retour: Commande[]) =>
      {
        let liste: any[] = [];

        this.listeCommande.set(liste.concat(this.listeCommande(), retour));
      }
    });
  }

  protected OuvrirModalRecette(_e:any): void
  {
    console.log(_e);
    
  }

  protected Suivant(): void
  {
    this.dateJour.update(x => 
    {
      if(this.mode() == EModeCalendrier.Semaine)
        x.ajouterJour(7);
      else if (this.mode() == EModeCalendrier.Mois)
        x.ajouterMois(1);
      else
        x.ajouterJour(1);

      return x;
    });

    this.ListerCommande();
  }

  protected Precedent(): void
  {
    this.dateJour.update(x => 
    {
      if(this.mode() == EModeCalendrier.Semaine)
        x.ajouterJour(-7);
      else if (this.mode() == EModeCalendrier.Mois)
        x.ajouterMois(-1);
      else
        x.ajouterJour(-1);

      return x;
    });

    this.ListerCommande();
  }

  private ListerCommande(_status: EStatusCommande = EStatusCommande.Tout): void
  {
    let dateJour = undefined;
    let dateFin = undefined;

    if(this.mode() == EModeCalendrier.Semaine)
    {
      dateJour = this.dateJour().datePremierJourSemaine();
      dateFin = new Date(dateJour.getTime());
      dateFin.ajouterJour(6);
    }
    else if(this.mode() == EModeCalendrier.Mois)
    {
      dateJour = this.dateJour().debutMois();
      dateFin = this.dateJour().finMois();
    }
    else
    {
      dateJour = dateFin = this.dateJour();
    }

    const INFOS: CommandeFiltreExport =
    {
      dateDebut: dateJour,
      dateFin: dateFin,
      status: _status
    };
    
    this.commandeServ.Lister(INFOS).subscribe({
      next: (liste) =>
      {
        this.listeCommande.set(liste);
      }
    });
  }
}
