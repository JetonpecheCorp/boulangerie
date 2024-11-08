import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { PaginationExport } from '@model/exports/PaginationExport';
import { Ingredient } from '@model/Ingredient';
import { IngredientService } from '@service/Ingredient.service';
import { InputComponent } from "../../components/input/input.component";
import { ButtonComponent } from "../../components/button/button.component";
import { RecetteService } from '@service/Recette.service';
import { RecetteExport } from '@model/exports/RecetteExport';
import { Recette } from '@model/Recette';

@Component({
  selector: 'app-ajouter-modifier-recette',
  standalone: true,
  imports: [MatInputModule, ReactiveFormsModule, MatFormFieldModule, MatAutocompleteModule, MatDialogModule, MatListModule, MatButtonModule, InputComponent, ButtonComponent],
  templateUrl: './ajouter-modifier-recette.component.html',
  styleUrl: './ajouter-modifier-recette.component.scss'
})
export class AjouterModifierRecetteComponent implements OnInit
{
  protected dataSourceFiltrer = signal<Ingredient[]>([]);
  protected form: FormGroup;
  protected btnClicker = signal(false);
  protected modeModifier = signal(false);
  
  private dataSource = signal<Ingredient[]>([]);
  private ingredientServ = inject(IngredientService);
  private recetteServ = inject(RecetteService);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(MatDialogRef<AjouterModifierRecetteComponent>)
  private info = inject(MAT_DIALOG_DATA);
  private recetteAjouter = signal<Recette[]>([]);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      autoComplete: new FormControl(""),
      quantite: new FormControl(this.info?.recette?.quantite ?? 0, [Validators.required, Validators.min(0)])
    });

    this.modeModifier.set(this.info?.recette ? true : false);    

    if(this.modeModifier())
      return;

    this.Lister(); 

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

  protected Valider(_fermerModal: boolean): void
  {    
    if(this.form.invalid)
      return;

    if(this.modeModifier())
    {
      console.log(this.form);
      return;
      
      const INFOS: RecetteExport = {
        idPublicIngredient: this.info.recette.idPublic,
        idPublicProduit: this.info.idPublicProduit,
        quantite: this.form.value.quantite
      };

      this.recetteServ.ModifierQuantite(INFOS).subscribe({
        next: () =>
        {
          this.Fermer();
        }
      });
    }

    const INGREDIENT = this.dataSource()
      .find(x => x.nom == this.form.value.autoComplete);

    if(!INGREDIENT)
      return;

    this.btnClicker.set(true);

    const INFOS: RecetteExport = {
      idPublicIngredient: INGREDIENT.idPublic,
      idPublicProduit: this.info.idPublicProduit,
      quantite: this.form.value.quantite
    };

    this.recetteServ.Ajouter(INFOS).subscribe({
      next: () => 
      {
        this.recetteAjouter.update(x =>
        {
          const RECETTE = {
            idPublicIngredient: INFOS.idPublicIngredient,
            idPublicProduit: INFOS.idPublicProduit,
            quantite: INFOS.quantite,
            nomIngredient: INGREDIENT.nom,
            nomProduit : this.info.nomProduit
          };

          return [...x, RECETTE]; 
        });

        this.info.listeIdPublicIngredient.push(INFOS.idPublicIngredient);
        this.InitAutoComplete(this.dataSource());  
        this.form.reset(); 

        this.btnClicker.set(false);

        if(_fermerModal)
          this.Fermer();
      },
      error: () => this.btnClicker.set(false)
    });
  }

  protected Fermer(): void
  {
    if(this.modeModifier())
      this.dialogRef.close(this.form.value.quantite);

    else
      this.dialogRef.close(this.recetteAjouter());
  }

  private Lister(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 1_000_000
    };

    this.ingredientServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.InitAutoComplete(retour.liste);
      }
    });
  }

  private InitAutoComplete(_liste: Ingredient[])
  {
    let liste: Ingredient[] = [];

    for (const element of _liste) 
      {
        const INDEX = (this.info.listeIdPublicIngredient as string[])
          .findIndex(x => x == element.idPublic);

        if(INDEX == -1)
          liste.push(element);
      }
      
      this.dataSource.set(liste);
      this.dataSourceFiltrer.set(liste);
  }
}
