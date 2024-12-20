import { Component, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande } from '@model/Commande';

@Component({
  selector: 'app-calendrier-mois',
  standalone: true,
  imports: [],
  templateUrl: './calendrier-mois.component.html',
  styleUrl: './calendrier-mois.component.scss'
})
export class CalendrierMoisComponent implements OnInit, OnChanges
{
  jourClicker = output<Commande>();

  dateJour = model.required<Date>();
  listeCommande = model.required<Commande[]>();
  listeJourMois = signal<JourSemaine[]>([]);

  protected readonly LISTE_JOUR_SEMAINE = Date.listerNomJourSemaine();

  ngOnInit(): void 
  {
    this.InitMois();
  }

  ngOnChanges(changes: SimpleChanges): void 
  {
    this.listeCommande.set(changes["listeCommande"].currentValue);
    this.InitMois();  
  }

  protected ElementClicker(_commande: Commande): void
  {
    this.jourClicker.emit(_commande);
  }

  private InitMois(): void
  {
    let liste: JourSemaine[] = [];

    const ANNEE = this.dateJour().getFullYear();
    const MOIS = this.dateJour().getMonth();

    let dateDebutMois = new Date(ANNEE, MOIS, 1);
    let dateFinMois = new Date(ANNEE, MOIS + 1, 0);

    let dateDebutSemaine = dateDebutMois.datePremierJourSemaine();
    const NB_JOUR_DIFF = dateDebutSemaine.nbJourDiff(dateDebutMois);

    let date = new Date(dateDebutSemaine);

    for (let i = 0; i < NB_JOUR_DIFF; i++) 
    {
      liste.push({
        date: date.toLocaleDateString(),
        nom: ""
      });

      date.ajouterJour(1);
    }
 
    date = new Date(ANNEE, MOIS, 1);

    for (let i = 0; i < dateFinMois.getDate(); i++) 
    {
      liste.push({
        date: date.toLocaleDateString(),
        nom: date.nomJour()
      });

      date.ajouterJour(1);
    }

    this.listeJourMois.set(liste);    
  }
}
