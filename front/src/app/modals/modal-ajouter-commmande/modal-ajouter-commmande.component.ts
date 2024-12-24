import { Component, DestroyRef, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { ButtonComponent } from "../../components/button/button.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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

@Component({
  selector: 'app-modal-ajouter-commmande',
  standalone: true,
  imports: [ MatIconModule, MatButtonModule, MatAutocompleteModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatDialogModule, ButtonComponent, ReactiveFormsModule],
  templateUrl: './modal-ajouter-commmande.component.html',
  styleUrl: './modal-ajouter-commmande.component.scss'
})
export class ModalAjouterCommmandeComponent implements OnInit
{
  private inputQte = viewChild.required<ElementRef>("inputQte");
  private autoCompleteProduit = viewChild.required<ElementRef>("autoCompleteProduit");

  protected btnClicker = signal(false);
  protected dateJour = (new Date()).ajouterJour(1);
  protected form: FormGroup;

  protected dataSourceFiltrer = signal<ProduitLeger[]>([]);

  private produitServ = inject(ProduitService);
  private destroyRef = inject(DestroyRef);
  private dataSource = signal<ProduitLeger[]>([]);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    this.ListerProduit();

    this.form = new FormGroup({
      autoComplete: new FormControl(""),
      listeProduit: new FormControl<ProduitCommandeExport[]>([], [Validators.required]),
      date: new FormControl<string>(this.dateJour.toISOFormat(), [Validators.required])
    });  

    this.form.controls['autoComplete'].valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur?: string) =>
        {
          console.log(valeur);
          
          let liste = this.dataSource()
            .filter(option => option.nom.toLowerCase().includes(valeur?.toLocaleLowerCase() || ''));

          this.dataSourceFiltrer.set(liste);
        }
      });
  }

  protected AjouterProduit(): void
  {
    let quantite = +this.inputQte().nativeElement.value;
    let valeurAutoComplete = this.autoCompleteProduit().nativeElement.value;
    
    const PRODUIT = this.dataSource().find(x => x.nom == valeurAutoComplete);

    if(!PRODUIT)
    {
      this.toastrServ.error("Veuillez choisir un produit");
      return;
    }

    if(quantite <= 0)
    {
      this.toastrServ.error("La quantité doit être supérieur à zéro");
      return;
    }

    const INDEX = (this.form.value.listeProduit as ProduitCommandeExport[])
      .findIndex(x => x.idPublic == PRODUIT.idPublic);

    if(INDEX != -1)
    {
      this.toastrServ.error(`${PRODUIT.nom} est déjà sélectionné(e)`);
      return;
    }

    (this.form.value.listeProduit as ProduitCommandeExport[]).push({
      idPublic: PRODUIT.idPublic,
      quantite: quantite
    });

    this.inputQte().nativeElement.value = undefined;
    this.autoCompleteProduit().nativeElement.value = "";

    console.log(this.form.value);
  }

  protected Ajouter(): void
  {
    console.log(this.form.value);
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
      }
    });
  }
}
