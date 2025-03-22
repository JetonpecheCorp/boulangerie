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
import { ExportService } from '@service/Export.service';
import {MatTooltipModule} from '@angular/material/tooltip';
import { ImporterDonneeComponent } from '@modal/importer-donnee/importer-donnee.component';
import { ETypeRessourceImport } from '@enum/ETypeRessourceImport';
import { ClientService } from '@service/Client.service';
import { Client } from '@model/Client';
import { ModalInfoComponent } from './modal-info/modal-info.component';
import { AjouterModifierClientComponent } from '@modal/ajouter-modifier-client/ajouter-modifier-client.component';

@Component({
  selector: 'app-client',
  standalone: true,
  imports: [StopPropagationDirective, MatTooltipModule, MatTableModule, MatIconModule, MatButtonModule, MatDialogModule, ReactiveFormsModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatFormFieldModule, MatInputModule],
  templateUrl: './client.component.html',
  styleUrl: './client.component.scss'
})
export class ClientComponent 
{
  displayedColumns: string[] = ["nom", "mail", "telephone", "adresse", "adresseFacturation", "infoComplementaire", "action"];
  dataSource = signal<MatTableDataSource<Client>>(new MatTableDataSource());
  estEnChargement = signal(false);

  nbParPage = signal(0);
  total = signal(0);

  inputFormCtrl = new FormControl();

  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  private clientServ = inject(ClientService);
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

  protected OuvrirModalImporter(): void
  {
    this.matDialog.open(ImporterDonneeComponent, { 
      data: ETypeRessourceImport.Client
    });
  }

  protected OuvrirModal(_client?: Client): void
  {
    const DIALOG_REF = this.matDialog.open(AjouterModifierClientComponent, { data: _client });
  }

  protected OuvrirModalInfo(_info?: string): void
  {
    this.matDialog.open(ModalInfoComponent, { data: { info: _info }});
  }

  protected Exporter(): void
  {
    this.exportServ.Client();
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

    this.clientServ.Lister(INFOS).subscribe({
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
