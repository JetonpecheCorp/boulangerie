import { Component, inject, model, OnInit, output, signal } from '@angular/core';
import { Commande } from '@model/Commande';
import { CommandeService } from '@service/Commande.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss'
})
export class TestComponent implements OnInit
{
  elementClick = output();

  protected listeJourSemaine = signal<any[]>([]);
  protected info = signal<any[]>([]);
  private readonly LISTE_JOUR_SEMAINE = ["Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"];

  private commandeServ = inject(CommandeService);

  ngOnInit(): void 
  {
    this.listeJourSemaine.set(this.InitSemaine());
    this.ListerCommande();
  }

  protected ElementClicker(_element: any): void
  {
    this.elementClick.emit(_element);
  }

  private OrdonerInfo(_liste: Commande[]): void
  {
    _liste.sort((a, b) =>
    {
      return a.date.getTime() - b.date.getTime();
    });

    let liste: any[] = [];

    for (let i = 1; i <= 7; i++) 
    {
      liste.push(_liste.filter(x => x.date.getDay() == (i == 7 ? 0 : i)));
    }

    this.info.set(liste);
  }

  private InitSemaine()
  {
    let listeJour = [];

    let dateDebutSemaine = this.DatePremierJourSemaine(new Date());
    
    for (let i = 0; i < 7; i++) 
    {
      let date = new Date(dateDebutSemaine);
      date.setDate(date.getDate() + i);      
      
      listeJour.push({
        nom: this.LISTE_JOUR_SEMAINE[i],
        date: date.toLocaleDateString()
      });
    }
    
    return listeJour;
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
    let dateJour = this.DatePremierJourSemaine(new Date());
    let dateFin = new Date("2024-12-06");

    this.commandeServ.Lister(dateJour, dateFin).subscribe({
      next: (liste) =>
      {
        this.OrdonerInfo(liste);

        console.log(this.info());
        
      }
    })
  }
}
