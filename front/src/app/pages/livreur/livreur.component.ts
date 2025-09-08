import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ButtonComponent } from '@component/button/button.component';
import { LivraisonService } from '@service/Livraison.service';
import {MatSelectModule} from '@angular/material/select';
import { LivraisonCommande, LivraisonLivreur } from '@model/LivraisonLivreur';
import {ClipboardModule} from '@angular/cdk/clipboard';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from "@angular/material/button";
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommandeService } from '@service/Commande.service';
import { ConvertionEnum, EStatusCommande } from '@enum/EStatusCommande';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-livreur',
  imports: [MatTooltipModule, MatIconModule, ClipboardModule, MatCardModule, ButtonComponent, MatSelectModule, MatButtonModule],
  templateUrl: './livreur.component.html',
  styleUrl: './livreur.component.scss'
})
export class LivreurComponent implements OnInit
{
  protected btnClicker = signal(false);

  protected listeLivraison = signal<LivraisonLivreur[]>([]);
  protected livraison = signal<LivraisonCommande[]>([]);
  private livraisonServ = inject(LivraisonService);
  private commandeServ = inject(CommandeService);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    this.Lister();  
  }

  protected LivraisonChoisi(_index: number): void
  {
    this.livraison.set(this.listeLivraison()[_index].listeCommande);
  }

  protected LivraisonFaite(_commande: LivraisonCommande): void
  {
    if(this.btnClicker())
      return;

    this.btnClicker.set(true);

    this.commandeServ.ModifierStatus(_commande.numero, EStatusCommande.Livrer).subscribe({
      next: () =>
      {
        _commande.status = EStatusCommande.Livrer;
        _commande.nomStatus = ConvertionEnum.StatusCommande(_commande.status);

        this.toastrServ.success("Commande mise à jour");

        this.btnClicker.set(false);
      },
      error: () => this.btnClicker.set(false)
    });
  }

  private Lister(): void
  {
    this.livraisonServ.ListerPourLivreur(new Date()).subscribe({
      next: (retour) =>
      {
        this.listeLivraison.set(retour);
      }
    });
  }
}
