import { AfterViewInit, Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { IngredientService } from '@service/Ingredient.service';
import { merge } from 'rxjs';
import { Ingredient } from '../../../models/Ingredient';

@Component({
  selector: 'app-ingredient',
  standalone: true,
  imports: [MatTableModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './ingredient.component.html',
  styleUrl: './ingredient.component.scss'
})
export class IngredientComponent implements OnInit, AfterViewInit
{
  displayedColumns: string[] = ["nom", "idPublic"];
  dataSource: MatTableDataSource<Ingredient>;
  estEnChargement = signal(false);

  ingredentServ = inject(IngredientService);

  nbParPage: number;
  total: number;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit(): void 
  {
    this.dataSource = new MatTableDataSource();
  }

  ngAfterViewInit(): void
  {
    this.dataSource.sort = this.sort;

    // declancher si le trié
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    // declancher si un des deux event est joué
    merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.Lister())
  }
  
  protected Lister(): void
  {
    this.ingredentServ.Lister(this.paginator.pageIndex + 1, this.paginator.pageSize).subscribe({
      next: (retour) =>
      { 
        this.dataSource.data = [];
        this.dataSource.data = retour.liste;

        this.total = retour.total;
        this.nbParPage = retour.nbParPage;
      }
    });
  }

  protected Rechercher(event: Event) 
  {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
