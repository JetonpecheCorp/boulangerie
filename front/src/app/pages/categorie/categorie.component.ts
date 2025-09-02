import { AfterViewInit, Component, DestroyRef, inject, signal, viewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { debounceTime, merge } from 'rxjs';
import { PaginationExport } from '@model/exports/PaginationExport';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CategorieService } from '@service/Categorie.service';
import { Categorie } from '@model/Categorie';
import { AjouterModifierCategorieComponent } from '@modal/ajouter-modifier-categorie/ajouter-modifier-categorie.component';
import { ThemeService } from '@service/ThemeService.Service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-categorie',
    imports: [MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
    templateUrl: './categorie.component.html',
    styleUrl: './categorie.component.scss'
})
export class CategorieComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "action"];
  dataSource = signal<MatTableDataSource<Categorie>>(new MatTableDataSource());
  estEnChargement = signal(false);

  categorieServ = inject(CategorieService);
  themeServ = inject(ThemeService);
  toastrServ = inject(ToastrService);
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
    this.paginator()._intl.itemsPerPageLabel = "Categorie par page";

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

  protected OuvrirModal(_categorie?: Categorie): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierCategorieComponent, {
      data: _categorie
    });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((retour?: Categorie) =>
      {        
        // modification
        if(retour && _categorie)
        {
          _categorie.nom = retour.nom;
        }
        // ajout
        else if(!_categorie && retour)
          this.Lister();
      });
  }

  protected OuvrirModalConfirmation(_categorie: Categorie): void
  {
    this.themeServ.OuvrirConfirmation(
      "Supression catégorie", 
      "Confirmez-vous la suppression de la catégorie ?"
    );

    this.themeServ.retourConfirmation.subscribe({
      next: (retour) =>
      {
        if(!retour)
          return;

        this.Supprimer(_categorie.idPublic);
      }
    });
  }

  private Supprimer(_idPublicCategorie: string): void
  {
    this.categorieServ.Supprimer(_idPublicCategorie).subscribe({
      next: () =>
      {
        this.dataSource.update(x => 
        {
          x.data = x.data.filter(x => x.idPublic != _idPublicCategorie);

          return x;
        });

        this.toastrServ.success("La catégorie a été supprimée");
      }
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

    this.categorieServ.ListerPaginer(INFOS).subscribe({
      next: (retour) =>
      {
        this.dataSource.update(x =>
        {
          x.data = retour.liste;
          return x;
        });

        this.total.set(retour.total);
        this.nbParPage.set(retour.nbParPage);
        this.estEnChargement.set(false);
      },
      error: () => this.estEnChargement.set(false)
    });
  }
}
