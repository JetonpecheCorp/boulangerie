import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerInputEvent, MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommandeFiltreExport, PaginationExport } from '@model/exports/PaginationExport';
import { UtilisateurLeger } from '@model/Utilisateur';
import { CommandeService } from '@service/Commande.service';
import { UtilisateurService } from '@service/Utilisateur.service';
import { EStatusCommande } from '../../../enums/EStatusCommande';
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
import { StopPropagationDirective } from '@directive/stop-propagation.directive';
import { Livraison } from '@model/Livraison';

type Info =
{
  date?: Date | null,
  livraison?: Livraison | null
}

@Component({
    selector: 'app-programmer-livraison',
    imports: [StopPropagationDirective, MatSelectModule, DragDropModule, MatIconModule, MatCardModule, MatDatepickerModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, MatInputModule, MatAutocompleteModule, MatFormFieldModule, ButtonComponent],
    templateUrl: './programmer-livraison.component.html',
    styleUrl: './programmer-livraison.component.scss'
})
export class ProgrammerLivraisonComponent implements OnInit
{
  protected dialogData: Info = inject(MAT_DIALOG_DATA);

  protected form: FormGroup;

  protected dataSourceClientFiltrer = signal<UtilisateurLeger[]>([]);
  
  private dataSourceClient = signal<UtilisateurLeger[]>([]);
  protected listeVehicule = signal<Vehicule[]>([]);
  protected listeCommande = signal<Commande[]>([]);
  protected listeCommandeLivraison = signal<Commande[]>([]);
  protected btnClicker = signal<boolean>(false);

  private dialogRef = inject(MatDialogRef<ProgrammerLivraisonComponent>);
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

    if(!this.dialogData?.date)
      this.dialogData = { ...this.dialogData, livraison: null };
    
    this.form = new FormGroup({
      autoCompleteConducteur: new FormControl<string>("", [Validators.required]),
      idPublicVehicule: new FormControl<string>("", [Validators.required]),
      fraisHT: new FormControl<number>(this.dialogData.livraison?.fraisHT ?? 0, [Validators.required, Validators.min(0)])
    });

    this.ListerCommande();

    if(this.dialogData?.livraison)
      this.DetailLivraison();

    this.ListerUtilisateur();
    this.ListerVehicule();

    this.form.controls["autoCompleteConducteur"].valueChanges
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

  protected AjouterOuModifier(): void
  {    
    if(this.listeCommandeLivraison().length == 0)
    {
      this.toastrServ.error("", "La liste des livraisons est vide");
      return;
    }

    if(this.btnClicker() || this.form.invalid)
      return; 

    const CONDUCTEUR = this.dataSourceClient().find(x => x.nomComplet == this.form.value.autoCompleteConducteur);

    if(!CONDUCTEUR)
      return;

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
      idPublicVehicule: this.form.value.idPublicVehicule,
      liste: listeCommande,
      frais: this.form.value.fraisHT
    };

    if(this.dialogData.livraison)
    {
      this.livraisonServ.Modifier(this.dialogData.livraison.idPublic, INFOS).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.listeCommandeLivraison.set([]);
  
          this.toastrServ.success("La livraison a été modifié");

          const LIVRAISON: Livraison = {
            numero: this.dialogData.livraison!.numero,
            date: this.dialogData.date!,
            fraisHT: this.form.value.fraisHT,
            idPublic: this.dialogData.livraison!.idPublic
          };
  
          this.dialogRef.close(LIVRAISON);
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      this.livraisonServ.Ajouter(INFOS).subscribe({
        next: (retour) =>
        {
          this.btnClicker.set(false);
          this.listeCommandeLivraison.set([]);
  
          this.toastrServ.success("La livraison a été ajouté");
  
          const LIVRAISON: Livraison = {
            numero: retour.numero,
            date: this.dialogData.date!,
            fraisHT: this.form.value.fraisHT,
            idPublic: retour.idPublic
          };
  
          this.dialogRef.close(LIVRAISON);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }

  private ListerVehicule(_idPublicVehiculeDefaut?: string): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 10_000_000
    };

    this.vehiculeServ.Lister(INFOS).subscribe({
      next: (retour) => 
      {
        this.listeVehicule.set(retour.liste);        

        if(_idPublicVehiculeDefaut)
          this.form.controls["idPublicVehicule"].setValue(_idPublicVehiculeDefaut);
      }
    });
  }

  private ListerCommande(): void
  {
    if(!this.dialogData.date)
      return;

    const INFOS: CommandeFiltreExport = 
    {
      nbParPage: 1_000_000,
      numPage: 1,
      dateDebut: this.dialogData.date!,
      dateFin: this.dialogData.date!,
      status: EStatusCommande.Valider,
      sansLivraison: true
    };

    this.commandeServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.listeCommande.set(retour.liste);  
      }
    });
  }

  private DetailLivraison(): void
  {    
    this.livraisonServ.Detail(this.dialogData.livraison!.idPublic).subscribe({
      next: (retour) =>
      {        
        this.listeCommandeLivraison.set(retour.listeCommande); 
        this.form.controls["autoCompleteConducteur"].setValue(retour.conducteur.nomComplet);   
        this.form.controls["idPublicVehicule"].setValue(retour.vehicule.idPublic);   
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
