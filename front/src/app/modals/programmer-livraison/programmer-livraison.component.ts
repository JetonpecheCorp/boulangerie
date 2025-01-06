import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
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
import {CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { ButtonComponent } from "../../components/button/button.component";
import { ToastrService } from 'ngx-toastr';
import { MatSelectModule } from '@angular/material/select';
import { Vehicule } from '@model/Vehicule';
import { LivraisonService } from '@service/Livraison.service';
import { LivraisonCommandeExport, LivraisonExport } from '@model/exports/LivraisonExport';
import { VehiculeService } from '@service/Vehicule.service';

type Info =
{
  date?: Date | null
}

@Component({
  selector: 'app-programmer-livraison',
  standalone: true,
  imports: [MatSelectModule, DragDropModule, MatIconModule, MatCardModule, MatDatepickerModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, MatInputModule, MatAutocompleteModule, MatFormFieldModule, ButtonComponent],
  templateUrl: './programmer-livraison.component.html',
  styleUrl: './programmer-livraison.component.scss'
})
export class ProgrammerLivraisonComponent implements OnInit
{
  protected dataSourceClientFiltrer = signal<UtilisateurLeger[]>([]);
  protected autoCompleteFormCtrl = new FormControl<string>("", [Validators.required]);
  protected vehiculeFormCtrl = new FormControl<string>("", [Validators.required]);

  protected dialogData: Info = inject(MAT_DIALOG_DATA);

  private dataSourceClient = signal<UtilisateurLeger[]>([]);
  protected listeVehicule = signal<Vehicule[]>([]);
  protected listeCommande = signal<Commande[]>([]);
  protected listeCommandeLivraison = signal<Commande[]>([]);
  protected btnClicker = signal<boolean>(false);

  private destroyRef = inject(DestroyRef);
  private utilisateurServ = inject(UtilisateurService);
  private commandeServ = inject(CommandeService);
  private livraisonServ = inject(LivraisonService);
  private vehiculeServ = inject(VehiculeService);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    if(!this.dialogData?.date)
      this.dialogData = { date: null };
    
    else
      this.ListerCommande();

    this.ListerUtilisateur();
    this.ListerVehicule();

    this.autoCompleteFormCtrl.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur?: string | null) =>
        {
          let liste = this.dataSourceClient()
            .filter(option => option.nomComplet.toLowerCase().includes(valeur?.toLocaleLowerCase() || ''));

          this.dataSourceClientFiltrer.set(liste);
        }
      });
  }

  protected Drop(_event: CdkDragDrop<Commande[]>): void
  {
    if (_event.previousContainer === _event.container) 
      moveItemInArray(_event.container.data, _event.previousIndex, _event.currentIndex);

    else 
    {
      transferArrayItem(
        _event.previousContainer.data,
        _event.container.data,
        _event.previousIndex,
        _event.currentIndex
      );
    }
  }

  protected ChangerDate(_date: MatDatepickerInputEvent<Date>): void
  {
    this.dialogData.date = _date.value;

    this.ListerCommande();
  }

  protected Ajouter(): void
  {
    if(this.listeCommandeLivraison().length == 0)
    {
      this.toastrServ.error("", "La liste des livraisons est vide");
      return;
    }

    if(this.btnClicker() || this.vehiculeFormCtrl.invalid || this.autoCompleteFormCtrl.invalid)
      return; 

    const CONDUCTEUR = this.dataSourceClient().find(x => x.nomComplet == this.autoCompleteFormCtrl.value);

    if(!CONDUCTEUR)
    {
      this.autoCompleteFormCtrl.setErrors({
        required: true
      });

      return;
    }

    this.btnClicker.set(true);

    let listeCommande: LivraisonCommandeExport[] = [];

    for (let i = 0; i < this.listeCommandeLivraison().length; i++) 
    {
      const element = this.listeCommandeLivraison()[i];

      listeCommande.push({
        numero: element.numero,
        ordre: i
      });
    }

    const INFOS: LivraisonExport = 
    {
      date: this.dialogData.date!.toISOFormat(),
      idPublicConducteur: CONDUCTEUR.idPublic,
      idPublicVehicule: this.vehiculeFormCtrl.value!,
      liste: listeCommande
    };

    this.livraisonServ.Ajouter(INFOS).subscribe({
      next: () =>
      {
        this.btnClicker.set(false);
        this.listeCommandeLivraison.set([]);

        this.toastrServ.success("La livraison a été ajouté");
      },
      error: () => this.btnClicker.set(false)
    });
  }

  private ListerVehicule(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 10_000_000
    };

    this.vehiculeServ.Lister(INFOS).subscribe({
      next: (retour) => 
      {
        this.listeVehicule.set(retour.liste);
      }
    });
  }

  private ListerCommande(): void
  {
    const INFOS: CommandeFiltreExport = 
    {
      dateDebut: this.dialogData.date!,
      dateFin: this.dialogData.date!,
      status: EStatusCommande.Valider,
      sansLivraison: true
    };

    this.commandeServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.listeCommande.set(retour);        
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
