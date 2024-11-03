import { AfterViewInit, Component, DestroyRef, inject, signal, viewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { IngredientService } from '@service/Ingredient.service';
import { debounceTime, merge } from 'rxjs';
import { Ingredient } from '@model/Ingredient';
import { PaginationExport } from '@model/exports/PaginationExport';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AjouterModifierIngredientComponent } from '@modal/ajouter-modifier-ingredient/ajouter-modifier-ingredient.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-ingredient',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './ingredient.component.html',
  styleUrl: './ingredient.component.scss'
})
export class IngredientComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "codeInterne", "stock", "stockAlert", "action"];
  dataSource = signal<MatTableDataSource<Ingredient>>(new MatTableDataSource());
  estEnChargement = signal(false);

  ingredentServ = inject(IngredientService);
  destroyRef = inject(DestroyRef);
  matDialog = inject(MatDialog);

  nbParPage = signal(0);
  total = signal(0);

  inputFormCtrl = new FormControl();

  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  ngAfterViewInit(): void
  {
    this.Lister();

    this.dataSource().sort = this.sort();
    this.paginator()._intl.itemsPerPageLabel = "Ingredient par page";

    // declancher si un des deux event est joué
    merge(this.inputFormCtrl.valueChanges, this.sort().sortChange)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.paginator().firstPage());

    merge(this.sort().sortChange, this.paginator().page)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.Lister());

    // event input recherche
    this.inputFormCtrl.valueChanges.pipe(
      debounceTime(300), 
      takeUntilDestroyed(this.destroyRef)
    )
    .subscribe(() =>this.Lister());
  }

  protected OuvrirModal(_ingredient?: Ingredient): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierIngredientComponent, {
      data: _ingredient
    });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((retour?: Ingredient) =>
      {        
        // modification
        if(retour && _ingredient)
        {
          _ingredient.nom = retour.nom;
          _ingredient.codeInterne = retour.codeInterne;
          _ingredient.stock = retour.stock;
          _ingredient.stockAlert = retour.stockAlert;
        }
        // ajout
        else if(!_ingredient && retour)
          this.Lister();
      });
  }

  private Lister(): void
  {
    this.estEnChargement.set(true);

    const INFOS: PaginationExport = { 
      nbParPage: this.paginator().pageSize,
      numPage: this.paginator().pageIndex + 1
    };

    if(this.inputFormCtrl.value)
      INFOS.thermeRecherche = this.inputFormCtrl.value

    this.ingredentServ.Lister(INFOS).subscribe({
      next: (retour) =>
      { 
        this.dataSource().data = [];
        this.dataSource().data = retour.liste;

        this.total.set(retour.total);
        this.nbParPage.set(retour.nbParPage);
        this.estEnChargement.set(false);
      },
      error: () =>this.estEnChargement.set(false)
    });
  }
}
