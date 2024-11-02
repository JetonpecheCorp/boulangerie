import { AfterViewInit, Component, DestroyRef, inject, OnInit, signal, viewChild } from '@angular/core';
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
import { ProduitService } from '@service/Produit.service';

@Component({
  selector: 'app-produit',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './produit.component.html',
  styleUrl: './produit.component.scss'
})
export class ProduitComponent implements OnInit, AfterViewInit
{
  displayedColumns: string[] = ["nom", "codeInterne", "stock", "stockAlert", "action"];
  dataSource: MatTableDataSource<any>;
  estEnChargement = signal(false);

  produitServ = inject(ProduitService);
  destroyRef = inject(DestroyRef);
  matDialog = inject(MatDialog);

  nbParPage: number;
  total: number;

  inputFormCtrl = new FormControl();

  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  ngOnInit(): void 
  {
    this.dataSource = new MatTableDataSource();
  }

  ngAfterViewInit(): void
  {
    this.Lister();

    this.dataSource.sort = this.sort();
    this.paginator()._intl.itemsPerPageLabel = "Produit par page";

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

  protected OuvrirModal(_produit?: any): void
  {

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

    this.produitServ.Lister(INFOS).subscribe({
      next: (retour) =>
      { 
        this.dataSource.data = [];
        this.dataSource.data = retour.liste;

        this.total = retour.total;
        this.nbParPage = retour.nbParPage;
        this.estEnChargement.set(false);
      },
      error: () =>this.estEnChargement.set(false)
    });
  }
}
