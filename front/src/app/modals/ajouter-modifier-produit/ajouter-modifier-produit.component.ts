import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { InputComponent } from "@component/input/input.component";
import { ProduitService } from '@service/Produit.service';
import { Produit } from '@model/Produit';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { TvaService } from '@service/Tva.service';
import { Tva } from '@model/Tva';
import { CategorieService } from '@service/Categorie.service';
import { Categorie } from '@model/Categorie';
import { MatSelectModule } from '@angular/material/select';
import { ButtonComponent } from "@component/button/button.component";

@Component({
    selector: 'app-ajouter-modifier-produit',
    imports: [MatDialogModule, MatSelectModule, MatIconModule, MatChipsModule, MatButtonModule, MatFormFieldModule, ReactiveFormsModule, InputComponent, ButtonComponent],
    templateUrl: './ajouter-modifier-produit.component.html',
    styleUrl: './ajouter-modifier-produit.component.scss'
})
export class AjouterModifierProduitComponent implements OnInit
{
  protected form: FormGroup;
  protected btnClicker = signal(false);
  protected labelBtn = signal("");
  protected listeAlergene = signal(this.matDialogData?.listeAllergene ?? []);
  protected listeTva = signal<Tva[]>([]);
  protected listeCategorie = signal<Categorie[]>([]);

  private announcer = inject(LiveAnnouncer);
  private produitServ = inject(ProduitService);
  private tvaServ = inject(TvaService);
  private categorieServ = inject(CategorieService);

  private dialogRef = inject(MatDialogRef<AjouterModifierProduitComponent>);
  private matDialogData?: Produit = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {
    this.labelBtn.set(this.matDialogData ? "Modifier" : "Ajouter");

    this.ListerTva();
    this.ListerCategorie();

    this.listeAlergene.set(this.matDialogData?.listeAllergene ?? []);

    this.form = new FormGroup({
      idPublic: new FormControl(this.matDialogData?.idPublic),
      nom: new FormControl<string>(this.matDialogData?.nom ?? "", [Validators.required, Validators.maxLength(200)]),
      codeInterne: new FormControl<string | null>(this.matDialogData?.codeInterne ?? null, [Validators.maxLength(100)]),
      stock: new FormControl<number>(this.matDialogData?.stock ?? 0),
      stockAlert: new FormControl<number>(this.matDialogData?.stockAlert ?? 0),
      prixHt: new FormControl<number>(this.matDialogData?.prixHt ?? 0, [Validators.required, Validators.min(0)]),
      poids: new FormControl<number | null>(this.matDialogData?.poids ?? null),
      listeAllergene: new FormControl<string[]>(this.matDialogData?.listeAllergene ?? []),
      idTva: new FormControl<number | undefined>(this.matDialogData?.tva.id ?? undefined, [Validators.required]),
      idPublicCategorie: new FormControl<string>(this.matDialogData?.categorie.idPublic ?? "", [Validators.required])
    });
  }

  protected Valider(): void
  {        
    if(this.form.invalid || this.btnClicker())
      return;

    this.form.value.listeAllergene = this.listeAlergene();
    
    this.btnClicker.set(true);
    
    if(this.matDialogData)
    {
      this.produitServ.Modifier(this.form.value).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close({ 
            produit: this.form.value,
            tvaValeur: this.listeTva().find(x => x.id == this.form.value.idTva)!.valeur, 
            nomCategorie: this.listeCategorie().find(x => x.idPublic == this.form.value.idPublicCategorie)!.nom
          });
        },
        error: () => this.btnClicker.set(false)
      });
    }
    else
    {
      delete this.form.value.idPublic;

      this.produitServ.Ajouter(this.form.value).subscribe({
        next: () =>
        {
          this.btnClicker.set(false);
          this.dialogRef.close(this.form.value);
        },
        error: () => this.btnClicker.set(false)
      });
    }
  }

  protected SupprimerTip(_mot: string) 
  {
    this.listeAlergene.update(liste => 
      {
      const INDEX = liste.indexOf(_mot);

      if (INDEX < 0)
        return liste;

      liste.splice(INDEX, 1);
      this.announcer.announce(`Suppression du tag ${_mot}`);
      return [...liste];
    });
  }

  protected AjouterTip(_event: MatChipInputEvent): void 
  {
    const MOT = (_event.value || '').trim();

    if (MOT) 
    {
      this.listeAlergene.update(liste => [...liste, MOT]);
      this.announcer.announce(`Ajout du tag ${MOT}`);
    }

    _event.chipInput!.clear();
  }

  private ListerTva(): void
  {
    this.tvaServ.Lister().subscribe({
      next: (retour) => 
      {
        this.listeTva.set(retour);
      }
    });
  }

  private ListerCategorie(): void
  {
    this.categorieServ.Lister().subscribe({
      next: (retour) =>
      {
        this.listeCategorie.set(retour);
      }
    });
  }
}
