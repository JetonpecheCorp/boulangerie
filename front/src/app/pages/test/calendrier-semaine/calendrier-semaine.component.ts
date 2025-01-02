import { Component, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande, ProduitCommande } from '@model/Commande';

type CommandeAlternatif =
{
  estActif: boolean,
  liste: ProduitCommande[]
}

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
  protected infoAlterntif = signal<CommandeAlternatif[]>([]);

  ngOnInit(): void 
  {
    this.listeJourSemaine.set(this.InitSemaine());
    this.InitListeCommande();
  }

  ngOnChanges(changes: SimpleChanges): void 
  {
    this.listeCommande.set(changes["listeCommande"].currentValue);

    this.listeJourSemaine.set(this.InitSemaine());
    this.InitListeCommande();
  }

  protected ElementClicker(_commande: Commande): void
  {
    this.commandeClicker.emit(_commande);
  }

  protected AfficherVueAlternatif(_indexJour: number): void
  {
    this.infoAlterntif.update(x => 
    {     
      const INFO = x[_indexJour];      

      INFO.estActif = !INFO.estActif;
        
      return x;
    });    
  }

  private InitListeCommande(): void
  {
    this.infoAlterntif.set([]);

    let liste: any[] = [];

    let date = this.dateJour().datePremierJourSemaine();

    for (let i = 1; i <= 7; i++) 
    {
      const LISTE = this.listeCommande().filter(x => x.date.getDay() == (i == 7 ? 0 : i) && date.toLocaleDateString() == x.date.toLocaleDateString());
      liste.push(LISTE);

      this.InitListeCommandeAlternatif(LISTE);

      date.ajouterJour(1);
    }

    this.info.set(liste);    
  }

  private InitListeCommandeAlternatif(_listeCommande: Commande[]): void
  {
    let listeCommandeAlternatif: ProduitCommande[] = [];
    
    for (const element of _listeCommande) 
    {
      for (const element2 of element.listeProduit) 
      {
        let info = listeCommandeAlternatif.find(x => x.idPublic == element2.idPublic);
        
        if(info)
          info.quantite += element2.quantite;

        else
          listeCommandeAlternatif.push({
            idPublic: element2.idPublic,
            nom: element2.nom,
            quantite: element2.quantite
          }); 
      }
    }

    this.infoAlterntif.update(x => [...x,{
      estActif: false,
      liste: listeCommandeAlternatif
    }]);
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
