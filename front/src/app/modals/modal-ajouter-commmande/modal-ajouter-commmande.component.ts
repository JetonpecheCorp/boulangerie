import { Component, DestroyRef, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ButtonComponent } from "../../components/button/button.component";
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ProduitService } from '@service/Produit.service';
import { PaginationExport } from '@model/exports/PaginationExport';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ProduitLeger } from '@model/Produit';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ProduitCommandeExport } from '@model/exports/ProduitCommandeExport';
import { ToastrService } from 'ngx-toastr';
import { ClientCommande, Commande, ProduitCommande } from '@model/Commande';
import { CommandeService } from '@service/Commande.service';
import { CommandeExport } from '@model/exports/CommandeExport';
import { ClientService } from '@service/Client.service';
import { ClientLeger } from '@model/Client';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { ConvertionEnum, EStatusCommande } from '../../../enums/EStatusCommande';

type Info = 
{
  date: Date,
  commande: Commande | null
}

@Component({
    selector: 'app-modal-ajouter-commmande',
    imports: [MatCheckboxModule, FormsModule, MatIconModule, MatButtonModule, MatAutocompleteModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatDialogModule, ButtonComponent, ReactiveFormsModule],
    templateUrl: './modal-ajouter-commmande.component.html',
    styleUrl: './modal-ajouter-commmande.component.scss'
})
export class ModalAjouterCommmandeComponent implements OnInit
{
  private inputQte = viewChild.required<ElementRef>("inputQte");

  protected estModeAjouter = signal(true);
  protected btnClicker = signal(false);
  protected form: FormGroup;

  protected dataSourceFiltrer = signal<ProduitLeger[]>([]);
  protected dataSourceClientFiltrer = signal<ClientLeger[]>([]);

  private dialogData: Info = inject(MAT_DIALOG_DATA);

  private dataSource = signal<ProduitLeger[]>([]);
  private dataSourceClient = signal<ClientLeger[]>([]);

  private produitServ = inject(ProduitService);
  private clientServ = inject(ClientService);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(MatDialogRef);
  private toastrServ = inject(ToastrService);
  private commandeServ = inject(CommandeService);

  ngOnInit(): void 
  {
    if(this.dialogData.commande)
      this.estModeAjouter.set(false);

    this.ListerProduit();
    this.ListerClient();

    this.form = new FormGroup({
      estLivraison: new FormControl(this.dialogData.commande?.estLivraison ?? false),
      autoComplete: new FormControl(""),
      autoCompleteClient: new FormControl<string | null>(this.dialogData.commande?.client?.nom ?? null),
      listeProduit: new FormControl<ProduitCommande[]>(this.dialogData.commande?.listeProduit ?? [], [Validators.required]),
      date: new FormControl<Date>(this.dialogData.date, [Validators.required])
    }); 
    
    this.form.controls['autoCompleteClient'].valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur?: string) =>
        {
          let liste = this.dataSourceClient()
            .filter(option => option.nom.toLowerCase().includes(valeur?.toLocaleLowerCase() || ''));

          this.dataSourceClientFiltrer.set(liste);
        }
      });

    this.form.controls['autoComplete'].valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur?: string) =>
        {          
          let liste = this.dataSource()
            .filter(option => option.nom.toLowerCase().includes(valeur?.toLocaleLowerCase() || ''));

          this.dataSourceFiltrer.set(liste);
        }
      });
  }

  protected AjouterProduit(): void
  {
    let quantite = +this.inputQte().nativeElement.value;
    let valeurAutoComplete = this.form.controls["autoComplete"].value;
    
    const PRODUIT = this.dataSource().find(x => x.nom == valeurAutoComplete);

    if(!PRODUIT)
    {
      this.toastrServ.error("Veuillez choisir un produit");
      return;
    }

    if(quantite <= 0)
      return;

    const INDEX = (this.form.value.listeProduit as ProduitCommandeExport[])
      .findIndex(x => x.idPublic == PRODUIT.idPublic);

    if(INDEX != -1)
    {
      this.toastrServ.error(`${PRODUIT.nom} est déjà sélectionné(e)`);
      return;
    }

    (this.form.value.listeProduit as ProduitCommande[]).push({
      idPublic: PRODUIT.idPublic,
      nom: PRODUIT.nom,
      quantite: quantite
    });

    this.inputQte().nativeElement.value = 1;
    this.form.controls["autoComplete"].reset();

    this.dataSource.update(x => {

      const INDEX = x.findIndex(y => y.idPublic == PRODUIT.idPublic);
      x.splice(INDEX, 1);

      return x;
    });

    this.dataSourceFiltrer.set(this.dataSource());
  }

  protected SupprimerProduit(_index: number, _produit: ProduitLeger)
  {
    this.form.value.listeProduit.splice(_index, 1);

    this.dataSourceFiltrer.update(x => [...x, _produit].sort((a, b) => {
      if(a.nom < b.nom)
        return -1;

      if(a.nom > b.nom)
        return 1;

      return 0;
    }));

    this.dataSource.set(this.dataSourceFiltrer());
  }

  protected Ajouter(): void
  {
    if(this.btnClicker())
      return;

    let listeProduit: ProduitCommandeExport[] = [];

    for (const element of this.form.controls["listeProduit"].value) 
    {
      listeProduit.push({
        idPublic: element.idPublic,
        quantite: element.quantite
      });
    }

    if(listeProduit.length == 0)
    {
      this.toastrServ.error("Il n'y a aucun produit");
      return;
    }

    const CLIENT = this.dataSourceClient().find(x => x.nom == this.form.value.autoCompleteClient);

    const INFOS: CommandeExport = {
      estLivraison: this.form.value.estLivraison,
      date: (this.form.controls["date"].value as Date).toISOFormat(),
      idPublicClient: CLIENT?.idPublic,
      listeProduit: listeProduit
    };

    this.btnClicker.set(true);

    if(this.dialogData.commande)
    {
      this.commandeServ.modifierAdmin(this.dialogData.commande.numero, INFOS).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close();
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      this.commandeServ.Ajouter(INFOS).subscribe({
        next: (numeroCommande) => 
        {
          const CLIENT = INFOS.idPublicClient ? 
          {
            idPublic: INFOS.idPublicClient, 
            nom: ""
          } as ClientCommande : null;
  
          const CMD: Commande = 
          {
            date: new Date(INFOS.date),
            client: CLIENT,
            livraison: null,
            status: EStatusCommande.EnAttenteValidation,
            nomStatus: ConvertionEnum.StatusCommande(EStatusCommande.EnAttenteValidation),
  
            estLivraison: this.form.value.estLivraison,
            numero: numeroCommande,
            listeProduit: this.form.controls["listeProduit"].value
          };
  
          this.btnClicker.set(false);
  
          this.dialogRef.close(CMD);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }

  private ListerProduit(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 10_000_000
    };

    this.produitServ.ListerLeger(INFOS).subscribe({
      next: (retour) =>
      {
        this.dataSource.set(retour.liste);
        this.dataSourceFiltrer.set(retour.liste);

        if(!this.dialogData.commande)
          return;

        let liste: ProduitLeger[] = [];
        let listeProduitCommande = this.dialogData.commande.listeProduit;

        for (const element of retour.liste) 
        {
          const INDEX = listeProduitCommande.findIndex(x => x.idPublic == element.idPublic);

          if(INDEX != -1)
            continue;

          liste.push({
            idPublic: element.idPublic,
            nom: element.nom,
            prixHt: element.prixHt
          });
        }

        this.dataSource.set(liste);
        this.dataSourceFiltrer.set(liste);
      }
    });
  }

  private ListerClient(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 10_000_000
    };

    this.clientServ.ListerLeger(INFOS).subscribe({
      next: (retour) => 
      {
        this.dataSourceClient.set(retour.liste);
        this.dataSourceClientFiltrer.set(retour.liste);
      }
    });
  }
}
