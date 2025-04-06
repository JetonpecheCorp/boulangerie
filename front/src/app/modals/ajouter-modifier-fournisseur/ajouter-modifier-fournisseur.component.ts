import { Component, inject, signal } from '@angular/core';
import { InputComponent } from "@component/input/input.component";
import { ButtonComponent } from "@component/button/button.component";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Fournisseur } from '@model/Fournisseur';
import { FournissseurService } from '@service/Fournisseur.service';
import { Ingredient } from '@model/Ingredient';
import { IngredientService } from '@service/Ingredient.service';
import { PaginationExport } from '@model/exports/PaginationExport';
import { ProduitService } from '@service/Produit.service';
import { Produit } from '@model/Produit';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { COMMA, ENTER } from '@angular/cdk/keycodes';

@Component({
  selector: 'app-ajouter-modifier-fournisseur',
  standalone: true,
  imports: [MatIconModule, MatChipsModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule, MatFormFieldModule, MatAutocompleteModule, ReactiveFormsModule, InputComponent, ButtonComponent],
  templateUrl: './ajouter-modifier-fournisseur.component.html',
  styleUrl: './ajouter-modifier-fournisseur.component.scss'
})
export class AjouterModifierFournisseurComponent 
{
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  
  protected form: FormGroup;
  protected btnClicker = signal(false);
  protected labelBtn = signal("");

  protected listeIngredientFiltrer = signal<Ingredient[]>([]);
  protected listeProduitFiltrer = signal<Produit[]>([]);

  protected listeIngredientChoisi = signal<Ingredient[]>([]);
  protected listeProduitChoisi = signal<Produit[]>([]);

  private listeIngredient = signal<Ingredient[]>([]);
  private listeProduit = signal<Produit[]>([]);

  private fournisseurServ = inject(FournissseurService);
  private ingredientServ = inject(IngredientService);
  private produitServ = inject(ProduitService);
  private dialogRef = inject(MatDialogRef<AjouterModifierFournisseurComponent>);
  private matDialogData?: Fournisseur = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {
    this.ListerIngredient();
    this.ListerProduit();
    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");

    this.form = new FormGroup({
      idPublic: new FormControl(this.matDialogData?.idPublic),
      nom: new FormControl<string>(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(300)]),
      adresse: new FormControl<string | null>(this.matDialogData?.adresse ?? null, [Validators.maxLength(800)]),
      mail: new FormControl<string | null>(this.matDialogData?.mail ?? null, [Validators.email]),
      telephone: new FormControl<string | null>(this.matDialogData?.telephone ?? null, [Validators.maxLength(20)])
    });
  }

  protected FiltrerIngredient(_valeur: string): void
  {
    let liste = this.listeIngredient()
          .filter(option => option.nom.toLowerCase().includes(_valeur.toLocaleLowerCase()));        

    this.listeIngredientFiltrer.set(liste);
  }

  protected FiltrerProduit(_valeur: string): void
  {
    let liste = this.listeProduit()
          .filter(option => option.nom.toLowerCase().includes(_valeur.toLocaleLowerCase()));        

    this.listeProduitFiltrer.set(liste);
  }

  protected IngredientChoisi(_valeur: string): void
  {
    const INDEX = this.listeIngredient().findIndex(y => y.nom == _valeur);

    this.listeIngredientChoisi.update((x) =>
    {
      x.push(this.listeIngredient()[INDEX]);

      return x;
    });

    this.listeIngredient.update((x) =>
    {
      x.splice(INDEX, 1);

      return x;
    });          
  }

  protected ProduitChoisi(_valeur: string): void
  {
    const INDEX = this.listeProduit().findIndex(y => y.nom == _valeur);

    this.listeProduitChoisi.update((x) =>
    {
      x.push(this.listeProduit()[INDEX]);

      return x;
    });

    this.listeProduit.update((x) =>
    {
      x.splice(INDEX, 1);

      return x;
    });
  }

  protected SupprimerIngredient(_ingredient: Ingredient, _index: number)
  {    
    this.listeIngredient.update((x) =>
    {
      x.push(_ingredient);

      return x.sort((a, b) => 
      {
        if(a.nom < b.nom)
          return -1;

        if(a.nom > b.nom)
          return 1;

        return 0;
      });
    });

    this.listeIngredientFiltrer.set(this.listeIngredient());

    this.listeIngredientChoisi.update((x) =>
    {
      x.splice(_index, 1);

      return x;
    }); 
  }

  protected SupprimerProduit(_produit: Produit, _index: number)
  {    
    this.listeProduit.update((x) =>
    {
      x.push(_produit);

      return x.sort((a, b) => 
      {
        if(a.nom < b.nom)
          return -1;

        if(a.nom > b.nom)
          return 1;

        return 0;
      });
    });

    this.listeProduitFiltrer.set(this.listeProduit());

    this.listeProduitChoisi.update((x) =>
    {
      x.splice(_index, 1);

      return x;
    }); 
  }

  protected Valider(): void
  {    
    if(this.form.invalid || this.btnClicker())
      return;
    
    this.btnClicker.set(true);

    this.form.value.listeIdPublicIngredient = this.listeIngredientChoisi().map(x => x.idPublic);
    this.form.value.listeIdPublicProduit = this.listeProduitChoisi().map(x => x.idPublic);

    if(this.matDialogData)
    {
      this.fournisseurServ.Modifier(this.matDialogData.idPublic, this.form.value).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      delete this.form.value.idPublic;

      this.fournisseurServ.Ajouter(this.form.value).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }

  private ListerIngredient(): void
  { 
    const INFOS: PaginationExport = {
      nbParPage: 1_000_000,
      numPage: 1
    };

    this.ingredientServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.listeIngredient.set(retour.liste);
        this.listeIngredientFiltrer.set(retour.liste);        
      }
    });
  }

  private ListerProduit(): void
  { 
    const INFOS: PaginationExport = {
      nbParPage: 1_000_000,
      numPage: 1
    };

    this.produitServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.listeProduit.set(retour.liste);
        this.listeProduitFiltrer.set(retour.liste);
      }
    });
  }
}
