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
import { Fournisseur } from '@model/Fournisseur';
import { FournissseurService } from '@service/Fournisseur.service';
import { AjouterModifierFournisseurComponent } from '@modal/ajouter-modifier-fournisseur/ajouter-modifier-fournisseur.component';

@Component({
  selector: 'app-fournisseur',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './fournisseur.component.html',
  styleUrl: './fournisseur.component.scss'
})
export class FournisseurComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "adresse", "mail", "telephone", "action"];
  dataSource = signal<MatTableDataSource<Fournisseur>>(new MatTableDataSource());
  estEnChargement = signal(false);

  fournisseurServ = inject(FournissseurService);
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
    this.paginator()._intl.itemsPerPageLabel = "Founisseur par page";

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

  protected OuvrirModal(_fournisseur?: Fournisseur): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierFournisseurComponent, {
      minWidth: "1000px",
      data: _fournisseur
    });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((retour?: Fournisseur) =>
      {        
        // modification
        if(retour && _fournisseur)
        {
          _fournisseur.nom = retour.nom;
          _fournisseur.adresse = retour.adresse;
          _fournisseur.mail = retour.mail;
          _fournisseur.telephone = _fournisseur.telephone;
        }
        // ajout
        else if(!_fournisseur && retour)
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

    this.fournisseurServ.Lister(INFOS).subscribe({
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
      error: () =>this.estEnChargement.set(false)
    });
  }
}
