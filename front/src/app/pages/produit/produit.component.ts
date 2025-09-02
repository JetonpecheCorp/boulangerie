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
import { ProduitService } from '@service/Produit.service';
import { Produit } from '@model/Produit';
import { AjouterModifierProduitComponent } from '@modal/ajouter-modifier-produit/ajouter-modifier-produit.component';
import { RecetteProduitComponent } from '@modal/recette-produit/recette-produit.component';
import { StopPropagationDirective } from '@directive/stop-propagation.directive';
import { ExportService } from '@service/Export.service';
import { ThemeService } from '@service/ThemeService.Service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-produit',
    imports: [StopPropagationDirective, MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
    templateUrl: './produit.component.html',
    styleUrl: './produit.component.scss'
})
export class ProduitComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "codeInterne", "prixHt", "poids", "stock", "stockAlert", "action"];
  dataSource = signal<MatTableDataSource<Produit>>(new MatTableDataSource());
  estEnChargement = signal(false);

  produitServ = inject(ProduitService);
  themeServ = inject(ThemeService);
  toastrServ = inject(ToastrService);
  exportServ = inject(ExportService);
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

  protected OuvrirModalRecetteProduit(_produit: Produit): void
  {
    this.matDialog.open(RecetteProduitComponent, { minWidth: "700px", data: {
        idPublicProduit: _produit.idPublic,
        nomProduit: _produit.nom
      }
    });
  }

  protected Exporter(): void
  {
    this.exportServ.Produit();
  }

  protected OuvrirModal(_produit?: Produit): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierProduitComponent, { data: _produit });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (retour?) =>
        {
          // modification
          if(retour && _produit)
            {
              _produit.nom = retour.produit.nom;
              _produit.codeInterne = retour.produit.codeInterne;
              _produit.stock = retour.produit.stock;
              _produit.stockAlert = retour.produit.stockAlert;
              _produit.poids = retour.produit.poids;
              _produit.prixHt = retour.produit.prixHt;
              _produit.listeAllergene = retour.produit.listeAllergene;
              _produit.categorie.idPublic = retour.produit.idPublicCategorie;
              _produit.categorie.nom = retour.nomCategorie;
              _produit.tva.id = retour.produit.idTva;
              _produit.tva.valeur = retour.tvaValeur;
            }
            // ajout
            else if(!_produit && retour)
              this.Lister();
        }
      });
  }

  protected OuvrirModalConfirmation(_produit: Produit): void
  {
    this.themeServ.OuvrirConfirmation(
      "Supression produit", 
      "Confirmez-vous la suppression du produit ?"
    );

    this.themeServ.retourConfirmation.subscribe({
      next: (retour) =>
      {
        if(!retour)
          return;

        this.Supprimer(_produit.idPublic);
      }
    });
  }

  private Supprimer(_idPublicProduit: string): void
  {
    this.produitServ.Supprimer(_idPublicProduit).subscribe({
      next: () =>
      {
        this.toastrServ.success("Le produit a été supprimé");
        
        this.dataSource.update(x =>
        {
          x.data = x.data.filter(y => y.idPublic != _idPublicProduit);

          return x;
        });
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

    this.produitServ.Lister(INFOS).subscribe({
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
