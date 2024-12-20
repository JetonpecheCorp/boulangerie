import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Commande } from '@model/Commande';
import { CommandeService } from '@service/Commande.service';
import { CalendrierSemaineComponent } from "./calendrier-semaine/calendrier-semaine.component";
import { CalendrierJourComponent } from "./calendrier-jour/calendrier-jour.component";

enum EModeCalendrier 
{
  Jour,
  Semaine
}

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatCardModule, CalendrierSemaineComponent, CalendrierJourComponent],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss'
})
export class TestComponent implements OnInit
{
  dateJour = signal(new Date());
  listeCommande = signal<Commande[]>([]);

  protected mode = signal(EModeCalendrier.Jour);
  protected eModeCalendrier = EModeCalendrier;

  private commandeServ = inject(CommandeService);

  ngOnInit(): void 
  {
    this.ListerCommande();
  }

  protected Suivant(): void
  {
    this.dateJour.update(x => 
    {
      if(this.mode() == EModeCalendrier.Semaine)
        x.setDate(x.getDate() + 7);
      else
        x.setDate(x.getDate() + 1);

      return x;
    });

    this.ListerCommande();
  }

  protected Precedent(): void
  {
    this.dateJour.update(x => 
    {
      if(this.mode() == EModeCalendrier.Semaine)
        x.setDate(x.getDate() - 7);
      else
        x.setDate(x.getDate() - 1);

      return x;
    });

    this.ListerCommande();
  }

  private DatePremierJourSemaine(d: Date) : Date
  {
    d = new Date(d);
    let day = d.getDay();
    
    // adjust when day is sunday
    let diff = d.getDate() - day + (day == 0 ? -6 : 1); 

    return new Date(d.setDate(diff));
  }

  private ListerCommande(): void
  {
    let dateJour = undefined;
    let dateFin = undefined;

    if(this.mode() == EModeCalendrier.Semaine)
    {
      dateJour = this.DatePremierJourSemaine(this.dateJour());
      dateFin = new Date(dateJour.getTime());
      dateFin.setDate(dateJour.getDate() + 6);
    }
    else
    {
      dateJour = dateFin = this.dateJour();
    }
    
    this.commandeServ.Lister(dateJour, dateFin).subscribe({
      next: (liste) =>
      {
        this.listeCommande.set(liste);
      }
    })
  }
}
