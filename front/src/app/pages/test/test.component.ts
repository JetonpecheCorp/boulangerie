import { Component, model, OnInit, output, signal } from '@angular/core';
import { ElementCalendrier } from '@model/Calendrier';

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

  Info = model<ElementCalendrier[]>([{
    date: new Date("2024-11-30"),
    nomProduit: "Pain",
    idPublicProduit: "",
    idPublic: "",
    quantite: 10
  },
  {
    date: new Date("2024-11-30"),
    nomProduit: "Pain au chocolat",
    idPublicProduit: "",
    idPublic: "",
    quantite: 5
  },
  {
    date: new Date("2024-11-25"),
    nomProduit: "Croissant",
    idPublicProduit: "",
    idPublic: "",
    quantite: 10
  }]);

  protected listeJourSemaine = signal<any[]>([]);
  protected info = signal<any[]>([]);
  private readonly LISTE_JOUR_SEMAINE = ["Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"];

  ngOnInit(): void 
  {
    this.listeJourSemaine.set(this.InitSemaine());
    this.OrdonerInfo();
  }

  protected ElementClicker(_element: any): void
  {
    this.elementClick.emit(_element);
  }

  private OrdonerInfo(): void
  {
    this.Info().sort((a, b) =>
    {
      return a.date.getTime() - b.date.getTime();
    });

    let liste: any[] = [];

    for (let i = 1; i <= 7; i++) 
    {
      liste.push(this.Info().filter(x => x.date.getDay() == (i == 7 ? 0 : i)));
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
}
