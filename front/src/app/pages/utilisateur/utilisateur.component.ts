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
import { StopPropagationDirective } from '@directive/stop-propagation.directive';
import { Utilisateur } from '@model/Utilisateur';
import { UtilisateurService } from '@service/Utilisateur.service';
import { ExportService } from '@service/Export.service';
import {MatTooltipModule} from '@angular/material/tooltip';
import { AjouterModifierUtilisateurComponent } from '@modal/ajouter-modifier-utilisateur/ajouter-modifier-utilisateur.component';
import { LivraisonComponent } from '../livraison/livraison.component';
import { ModalLivraisonComponent } from '@modal/modal-livraison/modal-livraison.component';

@Component({
  selector: 'app-utilisateur',
  standalone: true,
  imports: [StopPropagationDirective, MatTooltipModule, MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './utilisateur.component.html',
  styleUrl: './utilisateur.component.scss'
})
export class UtilisateurComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "prenom", "mail", "telephone", "estAdmin", "action"];
  dataSource = signal<MatTableDataSource<Utilisateur>>(new MatTableDataSource());
  estEnChargement = signal(false);

  nbParPage = signal(0);
  total = signal(0);

  inputFormCtrl = new FormControl();

  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  private utilisateurServ = inject(UtilisateurService);
  private exportServ = inject(ExportService);
  private destroyRef = inject(DestroyRef);
  private matDialog = inject(MatDialog);

  ngAfterViewInit(): void 
  {
    this.Lister();

    this.dataSource().sort = this.sort();
    this.paginator()._intl.itemsPerPageLabel = "Utilisateur par page";

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

  protected OuvrirModal(_utilisateur?: Utilisateur): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierUtilisateurComponent, { data: _utilisateur });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (retour?: Utilisateur) =>
        {
          // modification
          if(retour && _utilisateur)
          {
            _utilisateur.estAdmin = retour.estAdmin;
            _utilisateur.mail = retour.mail;
            _utilisateur.nom = retour.nom;
            _utilisateur.prenom;
            _utilisateur.telephone = retour.telephone;
          }
          // ajout
          else if(!_utilisateur && retour)
            this.Lister();
        }
      });
  }

  protected OuvrirModalLivraison(_utilisateur: Utilisateur): void
  {
    this.matDialog.open(ModalLivraisonComponent, { data: {
        idPublicConducteur: _utilisateur.idPublic
      },
      maxWidth: 2000
    });
  }

  protected Exporter(): void
  {
    this.exportServ.Utilisateur();
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

    this.utilisateurServ.Lister(INFOS).subscribe({
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
