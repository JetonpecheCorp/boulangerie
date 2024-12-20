import { Component, computed, model, OnInit, output, signal } from '@angular/core';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande } from '@model/Commande';

@Component({
  selector: 'app-calendrier-mois',
  standalone: true,
  imports: [],
  templateUrl: './calendrier-mois.component.html',
  styleUrl: './calendrier-mois.component.scss'
})
export class CalendrierMoisComponent implements OnInit
{
  commandeClicker = output<Commande>();

  dateJour = model.required<Date>();
  listeCommande = model.required<Commande[]>();
  listeJourMois = signal<JourSemaine[]>([]);

  protected readonly LISTE_JOUR_SEMAINE = ["Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"];

  ngOnInit(): void 
  {
    this.InitMois();
  }

  private InitMois(): void
  {
    let liste: JourSemaine[] = [];

    const ANNEE = this.dateJour().getFullYear();
    const MOIS = this.dateJour().getMonth();

    let dateDebutMois = new Date(ANNEE, MOIS, 1);
    let dateFinMois = new Date(ANNEE, MOIS + 1, 0);

    let dateDebutSemaine = this.DatePremierJourSemaine(dateDebutMois);
    let date = new Date(dateDebutSemaine);

    const NbJourDiff = this.NbJourDiff(dateDebutSemaine, dateDebutMois);

    for (let i = 0; i < NbJourDiff; i++) 
    {
      liste.push({
        date: date.toLocaleDateString(),
        nom: ""
      });

      date.setDate(date.getDate() + 1);
    }
 
    date = new Date(ANNEE, MOIS, 1);

    for (let i = 0; i < dateFinMois.getDate(); i++) 
    {
      const INDEX = date.getDay() == 0 ? 6 : date.getDay() - 1;

      liste.push({
        date: date.toLocaleDateString(),
        nom: this.LISTE_JOUR_SEMAINE[INDEX]
      });

      date.setDate(date.getDate() + 1);
    }

    this.listeJourMois.set(liste);

    console.log(liste);
    
  }

  private NbJourDiff(_dateDebut: Date, _dateFin: Date): number
  {
    const diffTime = Math.abs(_dateFin.getTime() - _dateDebut.getTime());
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24)); 

    return diffDays;
  }

  private DatePremierJourSemaine(d: Date) : Date
  {
    d = new Date(d);
    let day = d.getDay();
    
    // adjust when day is sunday
    let diff = d.getDate() - day + (day == 0 ? -6 : 1); 

    return new Date(d.setDate(diff));
  }
}
