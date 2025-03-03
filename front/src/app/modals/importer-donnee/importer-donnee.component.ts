import { Component, DestroyRef, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { ETypeRessourceImport } from '@enum/ETypeRessourceImport';
import { ErreurValidationCSV } from '@model/ErreurValidationCSV';
import { ImportService } from '@service/Import.service';
import { ToastrService } from 'ngx-toastr';
import { ButtonComponent } from "@component/button/button.component";

@Component({
  selector: 'app-importer-donnee',
  standalone: true,
  imports: [ReactiveFormsModule, MatDialogModule, MatButtonModule, MatSelectModule, MatFormFieldModule, ButtonComponent],
  templateUrl: './importer-donnee.component.html',
  styleUrl: './importer-donnee.component.scss'
})
export class ImporterDonneeComponent implements OnInit
{
  private inputFile = viewChild.required<ElementRef>("inputFile");
  private selectRessource = viewChild.required<MatSelect>("selectRessource");
  private typeRessource?: ETypeRessourceImport = inject(MAT_DIALOG_DATA);

  protected nomFichier = signal<string>("");
  protected eTypeRessourceImport = ETypeRessourceImport;
  protected listeErreur = signal<ErreurValidationCSV[]>([]);
  protected btnClicker = signal<boolean>(false);
  
  private destroyRef: DestroyRef = inject(DestroyRef);
  private importerServ = inject(ImportService);
  private toastrServ = inject(ToastrService);
  private dialogRef = inject(MatDialogRef<ImporterDonneeComponent>);

  ngOnInit(): void 
  {
    if(this.typeRessource)
      this.selectRessource().value = this.typeRessource;
  }

  protected Fichier(): void
  {
    this.nomFichier.set(this.inputFile().nativeElement.files[0].name);
  }

  protected Importer(): void
  {
    if(!this.selectRessource().value)
    {
      this.toastrServ.error("Choisir une ressource à importer");
      return;
    }

    if(this.inputFile().nativeElement.files.length == 0)
    {
      this.toastrServ.error("Aucun fichier");
      return;
    }

    this.importerServ.Importer(
      this.selectRessource().value,
      this.inputFile().nativeElement.files[0]
    )
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: (retour) =>
      {
        console.log(retour); 

        this.dialogRef.close({ import: true });
      }
    });
  }
}
