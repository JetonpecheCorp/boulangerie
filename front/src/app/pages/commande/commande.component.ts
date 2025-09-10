import { AfterContentInit, Component, DestroyRef, inject, input, OnInit, signal, viewChild } from '@angular/core';
import { CommandeFiltreExport, PaginationExport } from '@model/exports/PaginationExport';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
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
import { ButtonComponent } from "../../components/button/button.component";
import { ThemeService } from '@service/ThemeService.Service';
import { CommandeService } from '@service/Commande.service';
import { EStatusCommande } from '@enum/EStatusCommande';
import { Commande } from '@model/Commande';
import { ModalAjouterCommmandeComponent } from '@modal/modal-ajouter-commmande/modal-ajouter-commmande.component';
import { environment } from '../../../environments/environment';
import { ERole } from '@enum/ERole';

@Component({
  selector: 'app-commande',
  imports: [MatSortModule, MatButtonModule, MatPaginatorModule, MatTableModule, MatProgressSpinnerModule, ReactiveFormsModule, MatIconModule, MatDatepickerModule, MatAutocompleteModule, MatInputModule, MatFormFieldModule, ButtonComponent],
  templateUrl: './commande.component.html',
  styleUrl: './commande.component.scss'
})
export class CommandeComponent implements OnInit, AfterContentInit
{
  idPublicClient = input<string | undefined>();

  autoCompleteClientFormCtrl = new FormControl<string | null>(null);

  displayedColumns: string[] = ["numero", "date", "status", "action"];
  dataSource = signal<MatTableDataSource<Commande>>(new MatTableDataSource());
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

  private commandeServ = inject(CommandeService);
  private clientServ = inject(ClientService);
  private ToastrServ = inject(ToastrService);
  private themeServ = inject(ThemeService);
  private destroyRef = inject(DestroyRef);
  private matDialog = inject(MatDialog);

  ngOnInit(): void
  {
    this.dateJour.setHours(0, 0, 0, 0);

    this.form = new FormGroup({
      dateDebut: new FormControl<Date>(new Date()),
      dateFin: new FormControl<Date>(new Date().setFinMois())
    });

    this.ListerCommande();

    if(this.EstAdmin())
    {
      this.ListerClientLeger();
      this.autoCompleteClientFormCtrl.valueChanges
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (valeur: string | null) =>
          {
            let liste = this.listeClient()
              .filter(x => x.nom.toLowerCase().includes(valeur?.toLowerCase() || ""));

            this.listeClientFiltrer.set(liste);

            if(!valeur)
              this.ListerCommande();
          }
        });
    }

    this.paginator().page
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.ListerCommande());

    this.inputFormCtrl.valueChanges.pipe(
      debounceTime(300), 
      takeUntilDestroyed(this.destroyRef)
    )
    .subscribe(() => this.ListerCommande());
  }

  ngAfterContentInit(): void 
  {         
    this.dataSource().sort = this.sort();
    this.paginator()._intl.itemsPerPageLabel = "Commande par page";
  }

  protected EstAdmin(): boolean
  {
    return environment.utilisateur?.role == ERole.Admin;
  }

  protected OuvrirModalCommande(_commande?: Commande): void
  {
      let DIALOG_REF = this.matDialog.open(ModalAjouterCommmandeComponent, { data: { 
          date: this.dateJour,
          commande: _commande
        } 
      });

    DIALOG_REF.afterClosed().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (retour?: Commande) =>
      {
        if(!retour) 
          return;

        this.ListerCommande();
      }
    });
  }

  protected OuvrirModalConfirmation(_commande: Commande): void
  {
    const MESSAGE = `Confirmez-vous la suppression de la commande numero: ${_commande.numero} ?`;

    this.themeServ.OuvrirConfirmation("Suppression commande", MESSAGE);
    this.themeServ.retourConfirmation.subscribe({
      next: (retour) =>
      {
        if(retour)
          this.Supprimer(_commande.numero);
      }
    })
  }

  protected TelechargerFacture(_numeroCommande: string): void
  {
    this.commandeServ.TelechargerFacture(_numeroCommande);
  }

  protected ListerCommande(): void
  {
    this.estEnChargement.set(true);
    let idPublicClient = undefined;

    if(this.EstAdmin())
    {
      idPublicClient = this.idPublicClient() ?? this.listeClient()
        .find(x => x.nom == this.autoCompleteClientFormCtrl.value)?.idPublic;  
    }

    const INFOS: CommandeFiltreExport = 
    {
      nbParPage: this.paginator().pageSize ?? 5,
      numPage: this.paginator().pageIndex + 1,
      dateDebut: this.form.value.dateDebut,
      dateFin: this.form.value.dateFin,
      thermeRecherche: this.inputFormCtrl.value,
      status: EStatusCommande.Tout,
      idPublicClient: idPublicClient
    };

    this.commandeServ.Lister(INFOS).subscribe({
      next: (retour) =>
      {
        this.total.set(retour.total);
        this.dataSource.update(x => 
        {
          x.data = retour.liste;

          return x;
        });

        this.estEnChargement.set(false);
      },
      error: () => this.estEnChargement.set(false)
    });
  }

  private Supprimer(_numeroCommande: string): void
  {
    this.commandeServ.Supprimer(_numeroCommande).subscribe({
      next: () =>
      {
        this.dataSource.update(x =>
        {
          x.data = x.data.filter(y => y.numero != _numeroCommande);

          return x;
        });

        this.ToastrServ.success("La commande a été supprimée");
      }
    })
  }

  private ListerClientLeger(): void
  {
    const INFOS: PaginationExport = {
      nbParPage: 1_000_000,
      numPage: 1
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
