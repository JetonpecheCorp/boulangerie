import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { PaginationExport } from '@model/exports/PaginationExport';
import { UtilisateurLeger } from '@model/Utilisateur';
import { CommandeService } from '@service/Commande.service';
import { UtilisateurService } from '@service/Utilisateur.service';
import { EStatusCommande } from '../../../enums/EStatusCommande';
import { CommandeFiltreExport } from '@model/exports/CommandeExport';
import { Commande } from '@model/Commande';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';

type Info =
{
  date?: Date | null
}

@Component({
  selector: 'app-programmer-livraison',
  standalone: true,
  imports: [MatExpansionModule, MatIconModule, MatCardModule, MatDatepickerModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, MatInputModule, MatAutocompleteModule, MatFormFieldModule],
  templateUrl: './programmer-livraison.component.html',
  styleUrl: './programmer-livraison.component.scss'
})
export class ProgrammerLivraisonComponent implements OnInit
{
  protected dataSourceClientFiltrer = signal<UtilisateurLeger[]>([]);
  protected autoCompleteFormCtrl = new FormControl();

  protected dialogData: Info = inject(MAT_DIALOG_DATA);

  private dataSourceClient = signal<UtilisateurLeger[]>([]);
  protected listeCommande = signal<Commande[]>([]);

  private destroyRef = inject(DestroyRef);
  private utilisateurServ = inject(UtilisateurService);
  private commandeServ = inject(CommandeService);

  ngOnInit(): void 
  {
    if(!this.dialogData?.date)
      this.dialogData = { date: null };
    
    else
      this.ListerCommande();

    console.log(this.dialogData);

    this.ListerUtilisateur();

    this.autoCompleteFormCtrl.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur?: string) =>
        {
          let liste = this.dataSourceClient()
            .filter(option => option.nomComplet.toLowerCase().includes(valeur?.toLocaleLowerCase() || ''));

          this.dataSourceClientFiltrer.set(liste);
        }
      });
  }

  protected Test(_date: MatDatepickerInputEvent<Date>): void
  {
    this.dialogData.date = _date.value;

    this.ListerCommande();
  }

  private ListerCommande(): void
  {
    const INFOS: CommandeFiltreExport = {
      dateDebut: this.dialogData.date!,
      dateFin: this.dialogData.date!,
      status: EStatusCommande.Valider
    };

    this.commandeServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        const LISTE = retour.filter(x => x.estLivraison && x.livraison !== null);

        this.listeCommande.set(retour);
        console.log(this.listeCommande());
        
      }
    });
  }

  private ListerUtilisateur(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 10_000_000
    };

    this.utilisateurServ.ListerLeger(INFOS).subscribe({
      next: (retour) => 
      {
        this.dataSourceClient.set(retour.liste);
        this.dataSourceClientFiltrer.set(retour.liste);
      }
    });
  }
}
