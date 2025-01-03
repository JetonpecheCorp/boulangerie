import { Component, inject, model, OnChanges, OnInit, output, signal, SimpleChanges } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { JourSemaine } from '@model/calendrier/JourSemaine';
import { Commande, ProduitCommande } from '@model/Commande';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ProgrammerLivraisonComponent } from '@modal/programmer-livraison/programmer-livraison.component';

@Component({
  selector: 'app-calendrier-jour',
  standalone: true,
  imports: [MatCardModule, MatSlideToggleModule, MatButtonModule],
  templateUrl: './calendrier-jour.component.html',
  styleUrl: './calendrier-jour.component.scss'
})
export class CalendrierJourComponent implements OnInit, OnChanges
{
  commandeClicker = output<Commande>();

  dateJour = model.required<Date>();
  listeCommande = model.required<Commande[]>();

  listeCommandeAlternatif = signal<ProduitCommande[]>([]);

  protected vueAlternatifActiver = signal(false);
  protected jourSemaine = signal<JourSemaine>({ date: "", nom: "" });

  private matDialog = inject(MatDialog);

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

  protected OuvrirModalProgrammerLivraison(): void
  {
    this.matDialog.open(ProgrammerLivraisonComponent, { data: { date: this.dateJour() }}); 
  }

  protected Alternatif(): void
  {
    if(!this.vueAlternatifActiver())
      return;

    this.listeCommandeAlternatif.set([]);

    for (const element of this.listeCommande()) 
    {
      for (const element2 of element.listeProduit) 
      {
        let info = this.listeCommandeAlternatif().find(x => x.nom == element2.nom);
        
        if(info)
          info.quantite += element2.quantite;

        else
        {
          this.listeCommandeAlternatif.update(x => 
            [...x, { 
              idPublic: element2.idPublic,
              nom: element2.nom,
              quantite: element2.quantite 
              }]
          ); 
        }
      }
    }
  }

  protected ElementClicker(_commande: Commande): void
  {
    this.commandeClicker.emit(_commande);
  }

  private InitListeCommande(): void
  {    
    const LISTE = this.listeCommande().filter(x => x.date.getDay() == this.dateJour().getDay());

    this.listeCommande.set(LISTE);

    if(this.vueAlternatifActiver())
      this.Alternatif();
  }

  private InitJourSemaine(): void
  {
    this.jourSemaine.set({
      nom: this.dateJour().nomJour(),
      date: this.dateJour().toLocaleDateString()
    });
  }
}
