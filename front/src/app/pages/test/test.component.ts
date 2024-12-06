import { Component, inject, model, OnInit, output, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Commande } from '@model/Commande';
import { CommandeService } from '@service/Commande.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, MatCardModule],
  templateUrl: './test.component.html',
  styleUrl: './test.component.scss'
})
export class TestComponent implements OnInit
{
  elementClick = output();

  protected dateJour = signal(new Date());
  protected listeJourSemaine = signal<any[]>([]);
  protected info = signal<any[]>([]);
  private readonly LISTE_JOUR_SEMAINE = ["Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"];

  private commandeServ = inject(CommandeService);

  ngOnInit(): void 
  {
    this.listeJourSemaine.set(this.InitSemaine());
    this.ListerCommande();
  }

  protected Suivant(): void
  {
    this.dateJour.update(x => 
    {
      x.setDate(x.getDate() + 7)

      return x;
    });

    console.log(this.dateJour());
    this.listeJourSemaine.set(this.InitSemaine());
    this.ListerCommande();
  }

  protected Precedent(): void
  {
    this.dateJour.update(x => 
    {
      x.setDate(x.getDate() - 7)

      return x;
    });

    console.log(this.dateJour());

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

  private InitSemaine(): any[]
  {
    let listeJour = [];

    let dateDebutSemaine = this.DatePremierJourSemaine(this.dateJour());
    
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
    let dateJour = this.DatePremierJourSemaine(this.dateJour());
    let dateFin = new Date(dateJour.getTime());
    dateFin.setDate(dateJour.getDate() + 6);
    
    this.commandeServ.Lister(dateJour, dateFin).subscribe({
      next: (liste) =>
      {
        this.OrdonerInfo(liste);

        console.log(this.info());
        
      }
    })
  }
}
