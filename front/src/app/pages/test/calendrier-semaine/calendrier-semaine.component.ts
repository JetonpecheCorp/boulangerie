import { Component, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande } from '@model/Commande';

@Component({
  selector: 'app-calendrier-semaine',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './calendrier-semaine.component.html',
  styleUrl: './calendrier-semaine.component.scss'
})
export class CalendrierSemaineComponent implements OnInit, OnChanges
{
  commandeClicker = output<Commande>();

  dateJour = model.required<Date>();
  listeCommande = model.required<Commande[]>();

  protected listeJourSemaine = signal<JourSemaine[]>([]);
  protected info = signal<any[]>([]);

  ngOnInit(): void 
  {
    this.listeJourSemaine.set(this.InitSemaine());
    this.OrdonerInfo();
  }

  ngOnChanges(changes: SimpleChanges): void 
  {
    this.listeCommande.set(changes["listeCommande"].currentValue);

    this.listeJourSemaine.set(this.InitSemaine());
    this.OrdonerInfo();
  }

  protected ElementClicker(_commande: Commande): void
  {
    this.commandeClicker.emit(_commande);
  }

  private OrdonerInfo(): void
  {
    this.listeCommande().sort((a, b) =>
    {
      return a.date.getTime() - b.date.getTime();
    });

    let liste: any[] = [];

    for (let i = 1; i <= 7; i++) 
    {
      liste.push(this.listeCommande().filter(x => x.date.getDay() == (i == 7 ? 0 : i)));
    }

    this.info.set(liste);
  }

  private InitSemaine(): JourSemaine[]
  {
    let listeJour: JourSemaine[] = [];

    let dateDebutSemaine = this.dateJour().datePremierJourSemaine();
    
    for (let i = 0; i < 7; i++) 
    {
      let date = new Date(dateDebutSemaine);
      date.ajouterJour(i);      
      
      listeJour.push({
        nom: date.nomJour(),
        date: date.toLocaleDateString()
      });
    }
    
    return listeJour;
  }
}
