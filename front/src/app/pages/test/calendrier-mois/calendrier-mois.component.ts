import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { RetourCalendrierMois } from '@model/calendrier/RetourCalendrier';
import { Commande } from '@model/Commande';

type JourMois = {
  nom: string,
  date: string,
  nbCommande: number;
};

@Component({
    selector: 'app-calendrier-mois',
    imports: [],
    templateUrl: './calendrier-mois.component.html',
    styleUrl: './calendrier-mois.component.scss'
})
export class CalendrierMoisComponent implements OnInit, OnChanges
{
  jourClicker = output<RetourCalendrierMois>();

  dateJour = model.required<Date>();
  listeCommande =  model.required<Commande[]>();
  protected listeJourMois = signal<JourMois[]>([]);
  protected estPetiteTaille = signal(false);

  protected readonly LISTE_JOUR_SEMAINE = Date.listerNomJourSemaine();

    constructor(private breakpointObserver: BreakpointObserver) 
    {
      let breakpoint$ = this.breakpointObserver
        .observe([ '(max-width: 768px)']);
  
      breakpoint$.subscribe(() =>
        this.BreakpointChanges()
      );
    }

  ngOnInit(): void 
  {
    this.InitMois();
  }

  ngOnChanges(changes: SimpleChanges): void 
  {        
    this.listeCommande.set(changes["listeCommande"].currentValue);
    
    this.InitMois();  
  }

  protected ElementClicker(_date: string): void
  {    
    if(!_date)
      return;

    const LISTE = this.listeCommande().filter(x => x.date.toLocaleDateString() == _date);

    let date;

    if(_date.includes("/"))
    {
      let dateISO = _date.split("/").reverse().join("-");
      date = new Date(dateISO);
    }
    else
      date = new Date(_date);

    const INFO: RetourCalendrierMois = {
      listeCommande: LISTE,
      date: date
    }

    this.jourClicker.emit(INFO);
  }
  
  private BreakpointChanges(): void 
  {
    if (this.breakpointObserver.isMatched('(max-width: 768px)')) 
    {
      this.estPetiteTaille.set(true);
    } 
    else 
    {
      this.estPetiteTaille.set(false);
    }
  }

  private InitMois(): void
  {
    let liste: JourMois[] = [];

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
        date: "",
        nom: "",
        nbCommande: 0
      });

      date.ajouterJour(1);
    }
 
    date = new Date(ANNEE, MOIS, 1);

    for (let i = 0; i < dateFinMois.getDate(); i++) 
    {
      const NB_COMMANDE = this.listeCommande().filter(x => x.date.toLocaleDateString() == date.toLocaleDateString()).length;

      liste.push({
        date: date.toLocaleDateString(),
        nom: date.nomJour(),
        nbCommande : NB_COMMANDE
      });

      date.ajouterJour(1);
    }

    this.listeJourMois.set(liste);    
  }
}
