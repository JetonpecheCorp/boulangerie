import { AfterContentInit, Component, DestroyRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { PaginationExport, PaginationFiltreLivraisonExport } from '@model/exports/PaginationExport';
import { LivraisonService } from '@service/Livraison.service';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Livraison } from '@model/Livraison';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { ClientLeger } from '@model/Client';
import { ClientService } from '@service/Client.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { debounceTime } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ProgrammerLivraisonComponent } from '@modal/programmer-livraison/programmer-livraison.component';
import { ButtonComponent } from "../../components/button/button.component";
import { ThemeService } from '@service/ThemeService.Service';

@Component({
  selector: 'app-livraison',
  standalone: true,
  imports: [MatSortModule, MatButtonModule, MatPaginatorModule, MatTableModule, MatProgressSpinnerModule, ReactiveFormsModule, MatIconModule, MatDatepickerModule, MatAutocompleteModule, MatInputModule, MatFormFieldModule, ButtonComponent],
  templateUrl: './livraison.component.html',
  styleUrl: './livraison.component.scss'
})
export class LivraisonComponent implements OnInit, AfterContentInit
{
  autoCompleteClientFormCtrl = new FormControl<string | null>(null);

  displayedColumns: string[] = ["numero", "date", "fraisHT", "action"];
  dataSource = signal<MatTableDataSource<Livraison>>(new MatTableDataSource());
  estEnChargement = signal(false);

  listeClientFiltrer = signal<ClientLeger[]>([]);
  dateJour = new Date();

  nbParPage = signal(0);
  total = signal(0);

  inputFormCtrl = new FormControl();
  form: FormGroup;

  paginator = viewChild.required(MatPaginator);
  sort = viewChild.required(MatSort);

  private listeClient = signal<ClientLeger[]>([]);

  private livraisonServ = inject(LivraisonService);
  private clientServ = inject(ClientService);
  private ToastrServ = inject(ToastrService);
  private themeServ = inject(ThemeService);
  private destroyRef = inject(DestroyRef);
  private matDialog = inject(MatDialog);

  ngOnInit(): void
  {
    this.dateJour.setHours(0, 0, 0, 0);

    this.form = new FormGroup({
      dateDebut: new FormControl<Date | null>(new Date()),
      dateFin: new FormControl<Date | null>(new Date().setFinMois())
    });

    this.ListerClient();
    this.ListerLivraison();

    this.autoCompleteClientFormCtrl.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (valeur: string | null) =>
        {
          let liste = this.listeClient()
            .filter(x => x.nom.toLowerCase().includes(valeur?.toLowerCase() || ""));

          this.listeClientFiltrer.set(liste);
        }
      });

    this.paginator().page
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.ListerLivraison(true));

    this.inputFormCtrl.valueChanges.pipe(
      debounceTime(300), 
      takeUntilDestroyed(this.destroyRef)
    )
    .subscribe(() => this.ListerLivraison());
  }

  ngAfterContentInit(): void 
  {
    this.dataSource().sort = this.sort();
    this.paginator()._intl.itemsPerPageLabel = "Livraison par page";
  }

  protected OuvrirModalProgammerLivraison(): void
  {
    const DIALOG_REF = this.matDialog.open(ProgrammerLivraisonComponent, {
      maxWidth: 1200
    });

    DIALOG_REF.afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (retour: Livraison) =>
        {
          this.dataSource.update(x =>
          {
            if(x.data.length >= this.nbParPage())
              return x;

            if(retour.date <= this.form.value.dateFin || retour.date >= this.form.value.dateDebut)
              x.data.push(retour);

            return x;
          });

          this.dataSource().data = this.dataSource().data;
        }
    });
  }

  protected OuvrirModalSupprimerLivraison(_livraison: Livraison): void
  {
    const MESSAGE = `Confirmez-vous la suppression de la livraison numero: ${_livraison.numero} ?`;
    this.themeServ.OuvrirConfirmation("Suppression livraison", MESSAGE);
    
    this.themeServ.retourConfirmation.subscribe(retour => 
    {
      if(retour === false)
        return;

      this.SupprimerLivraison(_livraison.idPublic);
    });
  }

  protected OuvrirModalModifierLivraison(_livraison: Livraison): void
  {
    const DIALOG_REF = this.matDialog.open(ProgrammerLivraisonComponent, {
      maxWidth: 1200,
      data: {
        date: _livraison.date,
        livraison: _livraison
      }
    });
  }

  protected ListerLivraison(_eventPage: boolean = false): void
  {
    if(this.estEnChargement())
      return;

    this.estEnChargement.set(true);

    if(this.form.value.dateDebut && this.form.value.dateFin)
    {
      if(this.form.value.dateDebut > this.form.value.dateFin)
      {
        this.ToastrServ.error("La date de debut est supérieur à la date de fin");
        return;
      }
    }

    const ID_PUBLIC_CLIENT = this.listeClient()
      .find(x => x.nom == this.autoCompleteClientFormCtrl.value)?.idPublic;

    const INFOS: PaginationFiltreLivraisonExport =
    {
      numPage: _eventPage ? this.paginator().pageIndex + 1 : 1,
      nbParPage: this.paginator().pageSize ?? 20,

      dateDebut: this.form.value.dateDebut,
      dateFin: this.form.value.dateFin,

      thermeRecherche: this.inputFormCtrl.value,
      idPublicClient: ID_PUBLIC_CLIENT
    };

    this.livraisonServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.dataSource.update(x => 
        {
          x.data = retour.liste;

          return x;
        });

        this.estEnChargement.set(false);
        this.total.set(retour.total);
        this.nbParPage.set(retour.nbParPage);        
      },
      error: () => this.estEnChargement.set(false)
    })
  }

  private SupprimerLivraison(_idPublicLivraison: string): void
  {
    if(this.estEnChargement())
      return;

    this.estEnChargement.set(true);

    this.livraisonServ.Supprimer(_idPublicLivraison).subscribe({
      next: () =>
      {
        this.dataSource.update(x =>
        {
          const INDEX = x.data.findIndex(x => x.idPublic == _idPublicLivraison);

          if(INDEX == -1)
            return x;

          x.data.splice(INDEX, 1);

          return x;
        });

        this.ToastrServ.success("La livraison a été supprimé");
        this.estEnChargement.set(false);
      },
      error: () => this.estEnChargement.set(false)
    });
  }

  private ListerClient(): void
  {
    const INFOS: PaginationExport = {
      numPage: 1,
      nbParPage: 1_000_000
    };

    this.clientServ.ListerLeger(INFOS).subscribe({
      next: (retour) => 
      {
        this.listeClient.set(retour.liste);
        this.listeClientFiltrer.set(retour.liste);
      }
    });
  }
}
