import { AfterViewInit, Component, DestroyRef, inject, signal, viewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { debounceTime, merge } from 'rxjs';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { GroupeService } from '@service/Groupe.service';
import { Groupe } from '@model/Groupe';
import { AjouterModifierGroupeComponent } from '@modal/ajouter-modifier-groupe/ajouter-modifier-groupe.component';
import { ButtonComponent } from "@component/button/button.component";
import {MatTooltipModule} from '@angular/material/tooltip';

@Component({
    selector: 'app-groupe',
    imports: [MatTooltipModule, MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule, ButtonComponent],
    templateUrl: './groupe.component.html',
    styleUrl: './groupe.component.scss'
})
export class GroupeComponent implements AfterViewInit
{
  displayedColumns: string[] = ["nom", "adresse", "bloquer", "action"];
  dataSource = signal<MatTableDataSource<Groupe>>(new MatTableDataSource());
  estEnChargement = signal(false);
  btnClicker = signal(false);

  groupeServ = inject(GroupeService);
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
    this.paginator()._intl.itemsPerPageLabel = "Groupe par page";

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

  protected OuvrirModal(_groupe?: Groupe): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierGroupeComponent, {
      data: _groupe
    });

     DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((retour?: Groupe) =>
      {        
        // modification
        if(retour && _groupe)
        {
          _groupe.nom = retour.nom;
          _groupe.adresse = retour.adresse;
        }
        // ajout
        else if(!_groupe && retour)
          this.Lister();
      });
  }

  protected BloquerDebloquerConnexion(_groupe: Groupe): void
  {
    this.groupeServ.BloquerDebloquer(_groupe.id).subscribe({
      next: () =>
      {
        _groupe.connexionBloquer = !_groupe.connexionBloquer;
      }
    });
  }

  private Lister(): void
  {
    this.groupeServ.Lister().subscribe({
      next: (retour) =>
      {
        this.dataSource.update(x => 
        {
          x.data = retour;
          return x;
        });
      }
    });
  }
}
