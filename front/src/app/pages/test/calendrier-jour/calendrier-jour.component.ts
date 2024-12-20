import { Component, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande } from '@model/Commande';

@Component({
  selector: 'app-calendrier-jour',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './calendrier-jour.component.html',
  styleUrl: './calendrier-jour.component.scss'
})
export class CalendrierJourComponent implements OnInit, OnChanges
{
  commandeClicker = output<Commande>();

  dateJour = model.required<Date>();
  listeCommande = model.required<Commande[]>();

  protected jourSemaine = signal<JourSemaine>({ date: "", nom: "" });

  ngOnInit(): void 
  {
    this.InitJourSemaine();
    this.InitListeCommande();
  }

  ngOnChanges(changes: SimpleChanges): void 
  {
    this.listeCommande.set(changes["listeCommande"].currentValue);

    this.InitJourSemaine();
    this.InitListeCommande();
  }

  protected ElementClicker(_commande: Commande): void
  {
    this.commandeClicker.emit(_commande);
  }

  private InitListeCommande(): void
  {
    const LISTE = this.listeCommande().filter(x => x.date.getDay() == this.dateJour().getDay());

    this.listeCommande.set(LISTE);
  }

  private InitJourSemaine(): void
  {
    this.jourSemaine.set({
      nom: this.dateJour().nomJour(),
      date: this.dateJour().toLocaleDateString()
    });
  }
}
